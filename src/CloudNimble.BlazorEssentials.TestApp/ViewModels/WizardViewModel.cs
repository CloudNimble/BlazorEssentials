using CloudNimble.BlazorEssentials.Merlin;
using CloudNimble.BlazorEssentials.TestApp.Models;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace CloudNimble.BlazorEssentials.TestApp.ViewModels
{
    public class WizardViewModel : ViewModelBase<ConfigurationBase, AppState>
    {

        #region Properties

        public Operation Operation { get; set; }

        public Operation ModalOperation { get; set; }

        #endregion

        #region Constructors

        public WizardViewModel(ConfigurationBase configuration, AppState appState, NavigationManager navigationManager, IHttpClientFactory httpClientFactory) : base(navigationManager, httpClientFactory, configuration, appState)
        {
            var step1 = new OperationStep(1, "Sample operation: Making you wait...", async () => { await Task.Delay(5000); return await Task.FromResult(true); });
            var step2 = new OperationStep(1, "Sample operation: Making you wait...", async () => { await Task.Delay(5000); return await Task.FromResult(true); });

            Operation = new Operation("Static Wizard Operation", new List<OperationStep> { step1 }, "You did it!", "Something went wrong :(");
            ModalOperation = new Operation("Modal Wizard Operation", new List<OperationStep> { step2 }, "You did it!", "Something went wrong :(");
        }

        #endregion

        #region Public Methods

        #endregion

    }
}