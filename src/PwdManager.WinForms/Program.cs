using Microsoft.Extensions.DependencyInjection;
using PwdManager.WinForms.Composition;
using PwdManager.WinForms.Forms;

namespace PwdManager.WinForms;

internal static class Program
{
    [STAThread]
    private static void Main()
    {
        ApplicationConfiguration.Initialize();

        var configStore = Bootstrap.ConfigStore();
        var config = configStore.Load();

        bool ready = AppBootstrapper.IsReadyAsync(config).GetAwaiter().GetResult();
        if (!ready)
        {
            using var wizard = new SetupWizardForm(config);
            if (wizard.ShowDialog() != DialogResult.OK)
                return;
            config = configStore.Load();
        }

        ServiceProvider provider;
        try
        {
            provider = AppServices.Build(config);
        }
        catch (Exception ex)
        {
            MessageBox.Show("Servisler başlatılamadı:\n\n" + ex.Message,
                "PwdManager", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }

        using (provider)
        {
            var login = provider.GetRequiredService<LoginForm>();
            System.Windows.Forms.Application.Run(login);
        }
    }
}
