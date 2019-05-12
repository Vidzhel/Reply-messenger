//using PropertyChanged;
using System.ComponentModel;

namespace Server
{
    //[AddINotifyPropertyChangedInterface]
    public class BaseViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Call this to fire a PropertyChanged event
        /// </summary>
        /// <param name="name">name of property</param>
        public void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
