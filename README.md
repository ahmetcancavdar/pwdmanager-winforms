# PwdManager — Kurumsal Şifre Yönetimi

Windows masaüstü (WinForms / .NET 8), koyu tema (Guna.UI2.WinForms), MySQL.
İki rol: **Admin** ve **Personel**. Parolalar istemci tarafında **AES-256-GCM** ile
şifrelenir; veritabanına düz metin hiçbir zaman yazılmaz.

## Çözüm yapısı

| Proje | Sorumluluk |
|-------|-----------|
| `PwdManager.Core` | Kriptografi (Argon2id KDF, AES-256-GCM zarf şifreleme, kurtarma kodu), oturum bağlamı. Veritabanından bağımsız. |
| `PwdManager.Data` | **EF Core (Database First)** — `Sql/schema.sql` veritabanını tanımlar; `Entities/` + `Persistence/PwdManagerContext` bu şemadan `dotnet ef dbcontext scaffold` ile üretilir. `Repositories/` bu context üzerine yazılır (Pomelo + MariaDB). |
| `PwdManager.App`  | WinForms arayüz, koyu tema yöneticisi, DI bileşimi, kurulum sihirbazı |

### Veri katmanı — Database First akışı

1. `Sql/schema.sql` tek doğ­ru kaynaktır; şema değişince önce burası güncellenir ve MariaDB'ye uygulanır.
2. Entity/DbContext yeniden üretimi (`src/PwdManager.Data` dizininden):

   ```bash
   dotnet ef dbcontext scaffold "Server=127.0.0.1;Port=3306;Database=pwdmanager;User Id=root;Password=" \
     Pomelo.EntityFrameworkCore.MySql --context PwdManagerContext \
     --context-dir Persistence --output-dir Entities \
     --namespace PwdManager.Data.Entities --context-namespace PwdManager.Data.Persistence \
     --no-onconfiguring --force
   ```

3. Not: scaffold `app_meta` tablosunu `AppMetum` olarak adlandırır; bu ad `AppMetaRepository` içinde kapsüllenmiştir, dışarı sızmaz.
4. `is_active` / `must_change_pw` sütunları DB varsayılanı taşıdığı için `bool?` olarak üretilir; repository katmanı bunu net şekilde ele alır.

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

Veritabanını ele geçiren ancak kullanıcı parolalarını bilmeyen biri yalnızca
rastgele bloblar ve Argon2id özetleri görür — hiçbir parolayı çözemez.

**Kurtarma anahtarı:** Kurulumda bir kez üretilir ve gösterilir. DEK'in parola
bağımsız bir kopyasını sarar; tüm admin parolaları kaybolursa sistemi kurtarır.
Çevrimdışı (kağıt/kasa) saklanmalıdır.

## Roller

### Admin
- Parola ve kategori üzerinde tam CRUD (ekle / düzenle / sil)
- Personel hesabı oluşturma, parola sıfırlama, **Aktif/Pasif**: pasifleştirilen personelin
  açık oturumu ~2 sn içinde kapanır (giriş ekranına "Hesabınız devre dışı bırakıldı" ile döner)
  ve yeni giriş yapamaz
- Yetkilendirme: kategori bazında **veya** kategori içinde tek tek parola bazında
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

**Personel hesapları yalnızca admin tarafından açılır** ([PersonnelService](src/PwdManager.App/Services/PersonnelService.cs) `RequireAdmin`). Kendi kendine kayıt ekranı yoktur; giriş ekranı sadece kimlik doğrular.

## Arayüz

> **Her form ve UserControl iki dosyalıdır:** `X.Designer.cs` (yerleşim — Visual Studio
> tasarımcısında açılır ve sürükle-bırak ile düzenlenir; her sınıfın tasarımcı için
> parametresiz kurucusu vardır) + `X.cs` (olaylar, DI, iş mantığı). Koyu tema çalışma
> anında [`ThemeManager.Apply`](src/PwdManager.App/Theme/ThemeManager.cs) ile uygulanır
> (renk paleti tek kaynak). Izgara satırları / ağaç düğümleri / satır listesi gibi
> **veriyle dolan** kısımlar kodda kalır — WinForms'ta doğru desen budur.

- [Program.cs](src/PwdManager.App/Program.cs) — iki fazlı açılış: hazır değilse [SetupWizardForm](src/PwdManager.App/Forms/SetupWizardForm.cs), sonra [LoginForm](src/PwdManager.App/Forms/LoginForm.cs)
- [LoginForm](src/PwdManager.App/Forms/LoginForm.cs) → ilk girişte zorunlu [ChangePasswordForm](src/PwdManager.App/Forms/ChangePasswordForm.cs) → role göre shell
- **Admin shell** ([AdminShellForm](src/PwdManager.App/Forms/AdminShellForm.cs)) — sol menü + görünümler: [Kategoriler](src/PwdManager.App/Forms/Admin/CategoriesView.cs) · [Parolalar](src/PwdManager.App/Forms/Admin/SecretsView.cs) · [Personel](src/PwdManager.App/Forms/Admin/PersonnelView.cs) · [Yetkiler](src/PwdManager.App/Forms/Admin/PermissionsView.cs) (kategori/parola ağacı, onay kutuları anlık yazar) · [Denetim](src/PwdManager.App/Forms/Admin/AuditView.cs)
- **Personel shell** ([PersonnelShellForm](src/PwdManager.App/Forms/PersonnelShellForm.cs)) — salt okunur, **kategoriye göre gruplanmış tablo** ([PersonnelSecretsView](src/PwdManager.App/Forms/Personnel/PersonnelSecretsView.cs)): her kategori başlığı + sütun başlığı + satırlar ([SecretRowControl](src/PwdManager.App/Forms/Personnel/SecretRowControl.cs)). Erişilebilir parolası olmayan kategori hiç görünmez. Satıra çift tıkla → **satır yerinde açılır**, popup yok: giriş parolanı orada gir → şifre gösterilir → 20 sn geri sayım → maskelenir. ~15 sn'de bir yoklama, değişince yeniden çizer.

## Güvenlik önlemleri

- Tüm sorgular EF Core parametreli — enjeksiyona kapalı
- `appsettings.local.json`: DB parolası Windows DPAPI (CurrentUser) ile şifreli, git'e girmez
- MySQL kullanıcısı en az yetkili olmalı: yalnız bu şemada `SELECT/INSERT/UPDATE/DELETE`
- MySQL sunucusu yalnız özel ağ / `bind-address` ile sınırlandırılmalı
- Başarısız girişte hesap kilidi (5 deneme → 15 dk); reveal'da 3 deneme → iptal
- Açık parola diske/log'a yazılmaz; KEK/DEK kullanımı sonrası `CryptographicOperations.ZeroMemory`
- `audit_log`: login, görüntüleme (+reddedilen), ekle/düzenle/sil, yetki ver/al kayıtları
- Boşta otomatik kilit ([ShellFormBase](src/PwdManager.App/Forms/ShellFormBase.cs)): `IdleLockMinutes` (varsayılan 5) boyunca giriş yoksa shell kapanır, DEK temizlenir, giriş ekranına dönülür
- Servis katmanında `SessionContext.EnsureAdmin()` — personel oturumuyla yazma işlemi denenirse reddedilir (derinlemesine savunma)

## Geliştirme

Gereksinim: .NET 8 SDK (`global.json` 8.0.419'a sabitler), MySQL 8.x.

```bash
dotnet build PwdManager.sln
dotnet run --project src/PwdManager.App
```

İlk çalıştırmada kurulum sihirbazı açılır: DB bağlantısını test eder, `src/PwdManager.Data/Sql/schema.sql`
şemasını uygular, ilk admin hesabını oluşturur, DEK ve kurtarma anahtarını üretir.

## Masaüstü kısayolu ve çoklu pencere

```powershell
powershell -ExecutionPolicy Bypass -File .\publish-and-shortcut.ps1
```

`.\publish\PwdManager.App.exe` (tek dosya) üretir ve masaüstüne **PwdManager.lnk** kısayolu koyar.
Kod değişince yeniden çalıştır.

Uygulama **tek örnek kilidi kullanmaz**: kısayola üst üste tıklamak (veya bir pencerede
"Yeni pencere" / giriş ekranında "Yeni giriş penceresi") bağımsız kopyalar açar — böylece
iki (veya daha fazla) hesabı aynı anda yönetebilirsin. Her kopyanın kendi oturumu ve
kendi bellek-içi DEK'i vardır.
