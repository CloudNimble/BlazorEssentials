using CloudNimble.BlazorEssentials.Merlin;
using CloudNimble.BlazorEssentials.TestApp.Models;
using CloudNimble.EasyAF.Configuration;
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
            var operationSteps = new List<OperationStep> {
                new OperationStep(1, "Sample operation: Making you wait...", async () => { await Task.Delay(2000); return await Task.FromResult(true); }),
                new OperationStep(2, "Sample operation: Creating Randomly Generated Feature...", async () => { await Task.Delay(2000); return await Task.FromResult(true); }),
                new OperationStep(3, "Sample operation: Ensuring Everything Works Perfektly...", async () => { await Task.Delay(2000); return await Task.FromResult(true); }),
                new OperationStep(4, "Sample operation: Does Anyone Actually Read This?...", async () => { await Task.Delay(2000); return await Task.FromResult(true); }),
                new OperationStep(5, "Sample operation: Hitting Your Keyboard Won't Make This Faster...", async () => { await Task.Delay(2000); return await Task.FromResult(true); }),
            };

            var modalOperationSteps = new List<OperationStep> {
                new OperationStep(1, "Sample operation: Making you wait...", async () => { await Task.Delay(2000); return await Task.FromResult(true); }),
                new OperationStep(2, "Sample operation: Creating Randomly Generated Feature...", async () => { await Task.Delay(2000); return await Task.FromResult(true); }),
                new OperationStep(3, "Sample operation: Ensuring Everything Works Perfektly...", async () => { await Task.Delay(2000); return await Task.FromResult(true); }),
                new OperationStep(4, "Sample operation: Does Anyone Actually Read This?...", async () => { await Task.Delay(2000); return await Task.FromResult(true); }),
                new OperationStep(5, "Sample operation: Hitting Your Keyboard Won't Make This Faster...", async () => { await Task.Delay(2000); return await Task.FromResult(true); }),
            };

            Operation = new Operation("Static Wizard Operation", operationSteps, "You did it!", "Something went wrong :(");
            ModalOperation = new Operation("Modal Wizard Operation", modalOperationSteps, "You did it!", "Something went wrong :(");
        }

        #endregion

        #region Public Methods

        #endregion

    }
}