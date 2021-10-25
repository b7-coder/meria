using System;
namespace meria.MVVM.ViewModel.Base
{
    internal class LambdaCommand : Base.Command
    {
        private readonly Action<object> _Execate;
        private readonly Func<object, bool> _CanExecate;
        public LambdaCommand(Action<object> Execate, Func<object, bool> CanExecate = null)
        {
            _Execate = Execate ?? throw new ArgumentNullException(nameof(Execate));
            _CanExecate = CanExecate;
        }

        public override bool CanExecute(object parameter) => _CanExecate?.Invoke(parameter) ?? true;

        public override void Execute(object parameter) => _Execate(parameter);
    }
}
