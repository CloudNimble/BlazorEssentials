using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace CloudNimble.BlazorEssentials.Tests
{

    /// <summary>
    /// 
    /// </summary>
    [TestClass]
    public class AppStateBaseTests
    {

        #region Private Members

        Func<Task<bool>> trueAction = () => { Thread.Sleep(2000); return Task.FromResult(true); };
        Func<Task<bool>> falseAction = () => { Thread.Sleep(2000); return Task.FromResult(false); };

        #endregion

        /// <summary>
        /// Make sure that the Step is initialized properly.
        /// </summary>
        [TestMethod]
        public void AppStateBase_CorrectInitialState()
        {
            var state = new AppStateBase(null, null);
            state.LoadingStatus.Should().Be(LoadingStatus.NotLoaded);
        }

    }

}
