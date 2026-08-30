using Microsoft.Extensions.DependencyInjection;
using PwdManager.App.Composition;
using PwdManager.App.Forms;

namespace PwdManager.App;

internal static class Program
{
    [STAThread]
    private static void Main()
    {
        ApplicationConfiguration.Initialize();

        var config = ConfigStore.Load();

        bool ready = AppBootstrapper.IsReadyAsync(config).GetAwaiter().GetResult();
        if (!ready)
        {
            using var wizard = new SetupWizardForm(config);
            if (wizard.ShowDialog() != DialogResult.OK)
                return;
            config = ConfigStore.Load();
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
            Application.Run(login);
        }
    }
}
