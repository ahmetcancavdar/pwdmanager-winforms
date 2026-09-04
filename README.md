# PwdManager — Kurumsal Şifre Yönetimi

Windows masaüstü (WinForms / .NET 8), koyu tema (Guna.UI2.WinForms), **MySQL/MariaDB**.
İki rol: **Admin** ve **Personel**. Parolalar istemci tarafında **AES-256-GCM** ile
şifrelenir; veritabanına düz metin hiçbir zaman yazılmaz.

## Çözüm yapısı

Katmanlı (Clean Architecture) — bağımlılık yönü tek yönlü:
`WinForms → Application → Domain` ve `Infrastructure → Application (+ Domain)`.
**Application, Infrastructure'ı tanımaz**; **WinForms doğrudan veri katmanını bilmez**
(EF/Pomelo/DPAPI yalnızca Infrastructure ve WinForms'un bileşim kökünde adlandırılır).

| Proje | Sorumluluk |
|-------|-----------|
| `PwdManager.Domain` | Saf kurallar ve modeller: `SessionContext` / `SecretProtector` oturum bağlamı, `UserRole`, kriptografi **soyutlamaları** (`IPasswordHasher`, `IKeyDerivation`, `IDataProtector`, `IRecoveryCodeService`). DB/EF/WinForms yok. |
| `PwdManager.Application` | Use-case servisleri (`Services/` — Auth/Category/Secret/Personnel/Permission/Trash/Setup), repository **arayüzleri** (`Interfaces/Repositories.cs`), DTO/record'lar (`Models/Records.cs`), yapılandırma modeli (`Configuration/AppConfig.cs`), `AddApplication`. EF Core'a bağımlı değildir. |
| `PwdManager.Infrastructure` | **EF Core (Database First)** — `Sql/schema.sql` veritabanını tanımlar; `Entities/` + `Persistence/PwdManagerContext` bu şemadan `dotnet ef dbcontext scaffold` ile üretilir. `Repositories/` Application arayüzlerini uygular ve EF entity ↔ record eşlemesi yapar (`Mappers.cs`); `Security/` Argon2id/AES-GCM somut sınıfları; `Configuration/` DPAPI'li `ConfigStore` + `DatabaseBootstrapper`; `AddInfrastructure`. |
| `PwdManager.WinForms`  | WinForms arayüz, koyu tema yöneticisi, bileşim kökü (`Composition/` — `AppServices` katman DI uzantılarını birleştirir), kurulum sihirbazı. Formlar yalnızca Application tiplerini/record'larını görür; EF entity görmez. |

### Katman bağımlılık diyagramı

```mermaid
flowchart TD
    WF["PwdManager.WinForms<br/>Forms · Controls · Theme · Composition (bileşim kökü)"]
    APP["PwdManager.Application<br/>Services · Interfaces (repo + kripto) · Models/record · Config"]
    DOM["PwdManager.Domain<br/>SessionContext · SecretProtector · UserRole · kripto arayüzleri"]
    INF["PwdManager.Infrastructure<br/>EF Core · Repositories · Mappers · Argon2id/AES-GCM · DPAPI · schema.sql"]

    WF -->|"kullanır"| APP
    WF -.->|"yalnız bileşim kökünde:<br/>somut tipleri DI'a bağlar"| INF
    APP -->|"kullanır"| DOM
    INF -->|"arayüzleri uygular"| APP
    INF -->|"kullanır"| DOM

    classDef ui fill:#1e3a5f,stroke:#4a90d9,color:#fff
    classDef app fill:#1f4d2e,stroke:#4caf7d,color:#fff
    classDef dom fill:#4a3a1f,stroke:#d9a94a,color:#fff
    classDef inf fill:#4a1f3a,stroke:#d94a90,color:#fff
    class WF ui
    class APP app
    class DOM dom
    class INF inf
```

Ok tek yönlüdür. **Application, Infrastructure'ı bilmez** (EF/Pomelo/DPAPI oraya sızmaz);
**Domain hiçbir katmana bağımlı değildir**. WinForms'un Infrastructure'a tek dokunuşu
`Composition/`'da somut sınıfları DI kabına kaydetmesidir.

### Veri katmanı — Database First akışı

1. `Sql/schema.sql` tek doğ­ru kaynaktır; şema değişince önce burası güncellenir ve MariaDB'ye uygulanır.
2. Entity/DbContext yeniden üretimi (`src/PwdManager.Infrastructure` dizininden):

   ```bash
   dotnet ef dbcontext scaffold "Server=127.0.0.1;Port=3306;Database=pwdmanager;User Id=root;Password=" \
     Pomelo.EntityFrameworkCore.MySql --context PwdManagerContext \
     --context-dir Persistence --output-dir Entities \
     --namespace PwdManager.Infrastructure.Entities --context-namespace PwdManager.Infrastructure.Persistence \
     --no-onconfiguring --force
   ```

3. Not: scaffold `app_meta` tablosunu `AppMetum` olarak adlandırır; bu ad `AppMetaRepository` içinde kapsüllenmiştir, dışarı sızmaz.
4. `is_active` / `must_change_pw` sütunları DB varsayılanı taşıdığı için `bool?` olarak üretilir; repository katmanı bunu net şekilde ele alır.

## Veritabanı şeması (ER)

`schema.sql` — 9 tablo, InnoDB, `utf8mb4`. Şifreli alanlar `VARBINARY` (nonce‖ciphertext‖tag).

```mermaid
erDiagram
    users {
        bigint id PK
        varchar username UK
        enum role "Admin veya Personnel"
        varchar password_hash "Argon2id encoded"
        varbinary kdf_salt "KEK turetme tuzu"
        varbinary wrapped_dek "KEK ile sarilmis DEK"
        bool is_active
        bool must_change_pw
        int failed_login_count
        datetime locked_until
    }
    categories {
        bigint id PK
        varchar name "aktifler arasi benzersiz"
        bigint created_by FK
        datetime deleted_at "dolu ise Silinenler"
        bigint deleted_by FK
    }
    secrets {
        bigint id PK
        bigint category_id FK
        varchar title
        varbinary username_cipher "DEK ile sifreli kullanici adi"
        varbinary secret_cipher "DEK ile sifreli parola"
        bigint created_by FK
        datetime deleted_at "soft delete"
    }
    category_permissions {
        bigint user_id FK "bilesik PK user_id + category_id"
        bigint category_id FK
        bigint granted_by FK
    }
    secret_permissions {
        bigint user_id FK "bilesik PK user_id + secret_id"
        bigint secret_id FK
        bigint granted_by FK
    }
    secret_denies {
        bigint user_id FK "bilesik PK user_id + secret_id"
        bigint secret_id FK
        bigint denied_by FK "kategori verili iken istisna"
    }
    permission_sync {
        bigint user_id FK "PK"
        bigint version "her yetki degisiminde artar"
    }
    audit_log {
        bigint id PK
        bigint user_id "index var FK yok"
        varchar action "LOGIN SECRET_VIEW vb"
        varchar target_type
        bigint target_id
    }
    app_meta {
        varchar meta_key PK
        varbinary meta_value "recovery ve schema bilgileri"
    }

    users      ||--o{ categories           : "olusturan-silen"
    users      ||--o{ secrets              : "olusturan-silen"
    categories ||--o{ secrets              : "icerir cascade"
    users      ||--o{ category_permissions : "kategori erisimi"
    categories ||--o{ category_permissions : "verilen"
    users      ||--o{ secret_permissions   : "tekil erisim"
    secrets    ||--o{ secret_permissions   : "verilen"
    users      ||--o{ secret_denies        : "istisna"
    secrets    ||--o{ secret_denies        : "gizlenen"
    users      ||--o| permission_sync      : "yoklama sayaci"
    users      ||--o{ audit_log            : "kim gevsek bag"
```

- **Soft delete:** `categories.deleted_at` / `secrets.deleted_at`. Etkin görünürlük =
  `secrets.deleted_at IS NULL AND categories.deleted_at IS NULL`.
- **Cascade:** kategori kalıcı silinince altındaki `secrets` + tüm `*_permissions` / `secret_denies` de silinir.
- **`audit_log.user_id`** FK değil (kullanıcı silinse bile denetim kaydı kalsın diye) — yalnız index.

## Şifreleme mimarisi (zarf şifreleme)

```
DEK  (Data Encryption Key)  = 32 rastgele bayt. Sistemde TEK. Sadece bellekte açık durur.
KEK  (Key Encryption Key)   = Argon2id(kullanıcı_parolası, kdf_salt)  — her kullanıcı için ayrı
wrapped_dek                 = AES-256-GCM(KEK, DEK)  — users tablosunda saklanır

Parola kaydı:
  secret_cipher   = AES-256-GCM(DEK, parola)        (kayıt başına rastgele 12 baytlık nonce)
  username_cipher = AES-256-GCM(DEK, kullanıcı_adı)

Giriş:  parola -> KEK -> wrapped_dek çözülür -> DEK belleğe alınır (oturum boyunca)
Personel görüntüleme:  çift tıkla -> parolayı tekrar iste -> KEK -> DEK -> izni DB'den
                       yeniden doğrula -> secret_cipher çöz -> 20 sn göster -> maskele
```

### Anahtar hiyerarşisi

```mermaid
flowchart LR
    subgraph mem["Yalnızca bellek (oturum boyunca)"]
        DEK["DEK<br/>32 rastgele bayt<br/>sistemde TEK"]
    end

    PW["Kullanıcı parolası"] --> KEK
    SALT["kdf_salt<br/>(users)"] --> KEK["KEK<br/>Argon2id(parola, salt)"]
    KEK -->|"AES-256-GCM sarar"| WDEK["wrapped_dek<br/>users tablosu"]
    DEK -.->|"sarılan içerik"| WDEK

    RC["Kurtarma kodu<br/>kurulumda 1 kez"] --> RKEK["recovery KEK<br/>Argon2id(kod, recovery_salt)"]
    RKEK -->|"sarar"| RWDEK["recovery_wrapped_dek<br/>app_meta tablosu"]
    DEK -.->|"sarılan içerik"| RWDEK

    DEK ==>|"AES-256-GCM<br/>(kayıt başına rastgele nonce)"| SC["secret_cipher"]
    DEK ==>|"AES-256-GCM"| UC["username_cipher"]

    classDef key fill:#4a3a1f,stroke:#d9a94a,color:#fff
    classDef store fill:#1e3a5f,stroke:#4a90d9,color:#fff
    classDef cipher fill:#4a1f3a,stroke:#d94a90,color:#fff
    class DEK,KEK,RKEK key
    class WDEK,RWDEK store
    class SC,UC cipher
```

Kesikli oklar "bu anahtar şununla sarılıp saklanır" demektir; DEK diske hiçbir zaman
açık yazılmaz. `wrapped_dek` **her kullanıcı için ayrı** (admin kişiyi eklerken kendi
DEK'ini yeni kullanıcının parola‑KEK'i ile yeniden sarar) — böylece herkes aynı DEK'i
açar ama admin kimsenin parolasını görmez.

Veritabanını ele geçiren ancak kullanıcı parolalarını bilmeyen biri yalnızca
rastgele bloblar ve Argon2id özetleri görür — hiçbir parolayı çözemez.

**Kurtarma anahtarı:** Kurulumda bir kez üretilir ve gösterilir. DEK'in parola
bağımsız bir kopyasını sarar; tüm admin parolaları kaybolursa sistemi kurtarır.
Çevrimdışı (kağıt/kasa) saklanmalıdır.

## Roller

### Admin
- Parola ve kategori üzerinde tam CRUD. **Silme = soft delete**: kayıt DB'de kalır,
  arayüzden gizlenir; admin **Silinenler** sekmesinden geri yükleyebilir veya kalıcı
  silebilir. Etkin görünürlük = `secrets.deleted_at IS NULL AND categories.deleted_at IS NULL`;
  soft-silinen bir şey personele görünmez ve reveal edilemez (`ListVisibleSecrets` +
  `CanViewSecret` filtreli). Personel bu sekmeyi hiç görmez.
- Personel hesabı oluşturma, parola sıfırlama, **Aktif/Pasif**: pasifleştirilen personelin
  açık oturumu ~2 sn içinde kapanır (giriş ekranına "Hesabınız devre dışı bırakıldı" ile döner)
  ve yeni giriş yapamaz
- Yetkilendirme: kategori bazında **veya** kategori içinde tek tek parola bazında.
  Verili bir kategoride bir şifrenin kutusu kaldırılırsa **istisna (deny)** oluşur —
  kategori verili kalır ama o şifre o personelden gizlenir. Kategoriyi yeniden vermek
  istisnaları temizler. Etkin erişim = *(kategori verili ∪ şifre verili) ∧ ¬istisna*.
- Yetki/şifre/kategori değişiklikleri personelin listesine ~2 sn içinde yansır

### Personel
- Salt okunur. Yalnızca yetkilendirildiği parolaları, kategoriye göre gruplanmış tabloda görür
- Bir kategoride hiçbir şeye erişemiyorsa o kategori listede hiç görünmez
- Şifreyi görmek için satıra çift tıklar → satır yerinde açılır, giriş parolasını orada girer (3 hakkı vardır; aşınca o kayıt kilitlenir)
- Liste her ~2 sn'de bir yoklanır; **görünen küme değişmediyse arayüz yeniden çizilmez** (imza karşılaştırması), yalnızca gerçek değişimde render edilir
- Hesap pasifleştirilmişse yoklama bunu görür ve oturumu kapatır
- **Açık satır** her saniye izni + hesap durumunu yeniden sorgular: admin eş zamanlı olarak
  erişimi kaldırır **veya** personeli pasifleştirirse parola ~1 sn içinde gizlenir
- Başarısız yeniden-doğrulama denemeleri `audit_log`'a `REVEAL_AUTH_FAILED` olarak yazılır

**Personel hesapları yalnızca admin tarafından açılır** ([PersonnelService](src/PwdManager.Application/Services/PersonnelService.cs) → `SessionContext.EnsureAdmin()`). Kendi kendine kayıt ekranı yoktur; giriş ekranı sadece kimlik doğrular.

## Akış diyagramları

### 1) İlk kurulum (setup sihirbazı)

```mermaid
sequenceDiagram
    actor Admin
    participant Wiz as SetupWizardForm
    participant Boot as DatabaseBootstrapper
    participant DB as MySQL / MariaDB
    participant Setup as SetupService

    Admin->>Wiz: sunucu bilgisi + ilk admin (kullanıcı, parola)
    Wiz->>Boot: TestConnectionAsync
    Boot->>DB: bağlantı denemesi
    Wiz->>Boot: ApplySchemaAsync
    Boot->>DB: schema.sql (CREATE TABLE IF NOT EXISTS ...)
    Wiz->>Setup: CreateFirstAdminAsync(kullanıcı, parola, ad)
    Setup->>Setup: DEK = 32 rastgele bayt
    Setup->>Setup: adminKEK = Argon2id(parola, yeni salt)
    Setup->>DB: users INSERT (wrapped_dek = AES-GCM(adminKEK, DEK), role=Admin)
    Setup->>Setup: kurtarma kodu üret, recoveryKEK türet
    Setup->>DB: app_meta INSERT (recovery_salt, recovery_wrapped_dek, schema_version)
    Setup-->>Wiz: kurtarma kodu
    Wiz-->>Admin: kurtarma kodunu bir kez göster (çevrimdışı sakla)
```

### 2) Giriş ve oturum açılışı

```mermaid
sequenceDiagram
    actor U as Kullanıcı
    participant LF as LoginForm
    participant Auth as AuthService
    participant UR as IUserRepository
    participant DB as veritabanı

    U->>LF: kullanıcı adı + parola
    LF->>Auth: LoginAsync(kullanıcı, parola)
    Auth->>UR: FindByUsernameAsync
    UR->>DB: SELECT users
    alt kullanıcı yok / pasif / kilitli / parola yanlış
        Auth->>DB: audit LOGIN_FAILED, gerekiyorsa hesap kilidi (5 deneme -> 15 dk)
        Auth-->>LF: InvalidCredentials · Inactive · LockedOut
        LF-->>U: hata mesajı
    else parola doğru
        Auth->>Auth: KEK = Argon2id(parola, kdf_salt)
        Auth->>Auth: DEK = AES-GCM-unwrap(KEK, wrapped_dek)
        Auth->>DB: failed_login_count = 0 · audit LOGIN
        Auth-->>LF: Success + SessionContext (DEK yalnız bellekte)
        opt must_change_pw = true
            LF->>U: zorunlu ChangePasswordForm (DEK yeni parola-KEK'i ile yeniden sarılır)
        end
        LF->>U: role = Admin ? AdminShell : PersonnelShell
    end
```

### 3) Personel — parolayı görme (reveal) ve canlı iptal

```mermaid
sequenceDiagram
    actor P as Personel
    participant Row as SecretRowControl
    participant Auth as AuthService
    participant Sec as SecretService
    participant Perm as IPermissionRepository

    P->>Row: satıra çift tıkla
    Row-->>P: "Giriş parolanı gir" (satır yerinde açılır, popup yok)
    P->>Row: parola
    Row->>Auth: VerifyPasswordAsync(session, parola)
    Auth->>Auth: parolayı doğrula + DEK unwrap dene
    alt 3 hatalı deneme
        Auth->>Auth: audit REVEAL_AUTH_FAILED
        Row-->>P: bu kayıt için görüntüleme kilitlendi
    else parola doğru
        Row->>Sec: RevealAsync(session, secretId)
        Sec->>Perm: CanViewSecretAsync
        Note right of Perm: (kategori verili ∪ şifre verili)<br/>∧ ¬istisna ∧ ¬soft-silinmiş
        Perm-->>Sec: izin var
        Sec->>Sec: DEK ile secret_cipher / username_cipher çöz
        Sec->>Sec: audit SECRET_VIEW
        Sec-->>Row: RevealedSecret
        Row-->>P: parolayı göster + 20 sn geri sayım
        loop her 1 saniye (satır açıkken)
            Row->>Sec: CanRevealAsync(session, secretId)
            alt admin erişimi kaldırdı VEYA hesabı pasifleştirdi
                Sec-->>Row: false
                Row-->>P: "Erişiminiz kaldırıldı" — parola ~1 sn içinde gizlenir
            end
        end
    end
```

### 4) Etkin görünürlük / yetki kararı

Hem `ListVisibleSecrets` (liste) hem `CanViewSecret` (reveal öncesi ve her saniye) bu mantığı uygular:

```mermaid
flowchart TD
    S["Bu parolayı bu personel görebilir mi?"] --> ADM{"oturum admin mi?"}
    ADM -->|evet| YES["GÖRÜNÜR / REVEAL EDİLEBİLİR"]
    ADM -->|hayır| ACT{"hesap aktif mi?"}
    ACT -->|hayır| NO["GİZLİ"]
    ACT -->|evet| DEL{"secret veya kategori<br/>soft-silinmiş mi?"}
    DEL -->|evet| NO
    DEL -->|hayır| GRANT{"kategori verili<br/>VEYA şifre tek tek verili?"}
    GRANT -->|hayır| NO
    GRANT -->|evet| DENY{"secret_denies'de<br/>bu personel için istisna var mı?"}
    DENY -->|evet| NO
    DENY -->|hayır| YES

    classDef yes fill:#1f4d2e,stroke:#4caf7d,color:#fff
    classDef no fill:#4a1f1f,stroke:#d94a4a,color:#fff
    class YES yes
    class NO no
```

### 5) Canlı yansıma (yetki değişince personel ekranı)

```mermaid
sequenceDiagram
    actor A as Admin
    participant PS as PermissionService
    participant DB as veritabanı
    participant Shell as PersonnelShellForm
    participant View as PersonnelSecretsView

    A->>PS: kategori/şifre kutusunu değiştir
    PS->>DB: category_permissions / secret_permissions / secret_denies yaz
    PS->>DB: permission_sync.version += 1
    loop her ~2 saniye
        Shell->>DB: ListVisibleSecretsAsync(personelId)
        DB-->>Shell: güncel görünür küme
        Shell->>Shell: imza (id:kategori:başlık:tarih) değişti mi?
        alt değiştiyse
            Shell->>View: Render(yeni liste)
        else aynıysa
            Shell->>Shell: hiçbir şey yapma (titremeyen UI)
        end
    end
```

## Arayüz

> **Her form ve UserControl iki dosyalıdır:** `X.Designer.cs` (yerleşim — Visual Studio
> tasarımcısında açılır ve sürükle-bırak ile düzenlenir; her sınıfın tasarımcı için
> parametresiz kurucusu vardır) + `X.cs` (olaylar, DI, iş mantığı). Koyu tema çalışma
> anında [`ThemeManager.Apply`](src/PwdManager.WinForms/Theme/ThemeManager.cs) ile uygulanır
> (renk paleti tek kaynak). Izgara satırları / ağaç düğümleri / satır listesi gibi
> **veriyle dolan** kısımlar kodda kalır — WinForms'ta doğru desen budur.
>
> Not: Guna.UI2 ücretsizdir ancak Visual Studio **tasarımcısı** ilk açılışta bir kez
> ücretsiz lisans anahtarı ister ([guna.io](https://guna.io)). `dotnet build` / `dotnet run`
> ve yayınlanan exe lisans gerektirmez.

- [Program.cs](src/PwdManager.WinForms/Program.cs) — iki fazlı açılış: hazır değilse [SetupWizardForm](src/PwdManager.WinForms/Forms/SetupWizardForm.cs), sonra [LoginForm](src/PwdManager.WinForms/Forms/LoginForm.cs)
- [LoginForm](src/PwdManager.WinForms/Forms/LoginForm.cs) → ilk girişte zorunlu [ChangePasswordForm](src/PwdManager.WinForms/Forms/ChangePasswordForm.cs) → role göre shell
- **Admin shell** ([AdminShellForm](src/PwdManager.WinForms/Forms/AdminShellForm.cs)) — sol menü + görünümler: [Kategoriler](src/PwdManager.WinForms/Forms/Admin/CategoriesView.cs) · [Parolalar](src/PwdManager.WinForms/Forms/Admin/SecretsView.cs) · [Personel](src/PwdManager.WinForms/Forms/Admin/PersonnelView.cs) · [Yetkiler](src/PwdManager.WinForms/Forms/Admin/PermissionsView.cs) (kategori/parola ağacı, onay kutuları anlık yazar) · [Denetim](src/PwdManager.WinForms/Forms/Admin/AuditView.cs)
- **Personel shell** ([PersonnelShellForm](src/PwdManager.WinForms/Forms/PersonnelShellForm.cs)) — salt okunur, **kategoriye göre gruplanmış tablo** ([PersonnelSecretsView](src/PwdManager.WinForms/Forms/Personnel/PersonnelSecretsView.cs)): her kategori başlığı + sütun başlığı + satırlar ([SecretRowControl](src/PwdManager.WinForms/Forms/Personnel/SecretRowControl.cs)). Erişilebilir parolası olmayan kategori hiç görünmez. Satıra çift tıkla → **satır yerinde açılır**, popup yok: giriş parolanı orada gir → şifre gösterilir → 20 sn geri sayım → maskelenir. ~2 sn'de bir yoklama, görünen küme değişince yeniden çizer.

## Güvenlik önlemleri

- Tüm sorgular EF Core parametreli — enjeksiyona kapalı
- `appsettings.local.json`: DB parolası Windows DPAPI (CurrentUser) ile şifreli, git'e girmez
- MySQL kullanıcısı en az yetkili olmalı: yalnız bu şemada `SELECT/INSERT/UPDATE/DELETE`
- MySQL sunucusu yalnız özel ağ / `bind-address` ile sınırlandırılmalı
- Başarısız girişte hesap kilidi (5 deneme → 15 dk); reveal'da 3 deneme → iptal
- Açık parola diske/log'a yazılmaz; KEK/DEK kullanımı sonrası `CryptographicOperations.ZeroMemory`
- `audit_log`: login, görüntüleme (+reddedilen), ekle/düzenle/sil, yetki ver/al kayıtları
- Boşta otomatik kilit ([ShellFormBase](src/PwdManager.WinForms/Forms/ShellFormBase.cs)): `IdleLockMinutes` (varsayılan 5) boyunca giriş yoksa shell kapanır, DEK temizlenir, giriş ekranına dönülür
- Servis katmanında `SessionContext.EnsureAdmin()` — personel oturumuyla yazma işlemi denenirse reddedilir (derinlemesine savunma)

## Geliştirme

Gereksinim: .NET 8 SDK (`global.json` 8.0.419'a sabitler) · **MySQL 8.x veya MariaDB 10.4+**
(geliştirmede XAMPP/MariaDB 10.4 kullanıldı; Pomelo sağlayıcısı ikisini de destekler).

```bash
dotnet build PwdManager.sln
dotnet run --project src/PwdManager.WinForms
```

İlk çalıştırmada kurulum sihirbazı açılır: DB bağlantısını test eder, `src/PwdManager.Infrastructure/Sql/schema.sql`
şemasını uygular, ilk admin hesabını oluşturur, DEK ve kurtarma anahtarını üretir.

## Masaüstü kısayolu ve çoklu pencere

```powershell
powershell -ExecutionPolicy Bypass -File .\publish-and-shortcut.ps1
```

`.\publish\PwdManager.WinForms.exe` (tek dosya) üretir ve masaüstüne **PwdManager.lnk** kısayolu koyar.
Kod değişince yeniden çalıştır.

Uygulama **tek örnek kilidi kullanmaz**: masaüstü kısayoluna üst üste tıklamak her seferinde
bağımsız, kendi giriş ekranına sahip yeni bir kopya açar — böylece iki (veya daha fazla)
hesabı aynı bilgisayarda aynı anda yönetebilirsin. Her kopyanın kendi oturumu ve kendi
bellek-içi DEK'i vardır. (Arayüzde ayrı bir "yeni pencere" düğmesi yok; kısayola tekrar
tıklamak yeterli.)
