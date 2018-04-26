using System;
using System.Collections.ObjectModel;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;

namespace Try.Client.ViewModel
{
    /// <summary>
    /// This class contains properties that the main View can data bind to.
    /// <para>
    /// Use the <strong>mvvminpc</strong> snippet to add bindable properties to this ViewModel.
    /// </para>
    /// <para>
    /// You can also use Blend to data bind with the tool's support.
    /// </para>
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class MainViewModel : ViewModelBase
    {
        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel(IStudentService studentService)
        {
            StudentService = studentService;
            ////if (IsInDesignMode)
            ////{
            ////    // Code runs in Blend --> create design time data.
            ////}
            ////else
            ////{
            ////    // Code runs "for real"
            ////}
        }


        public IStudentService StudentService { get; set; }

        private ObservableCollection<Service.Application.Dto.Students.StudentDto> _studentDtos=
            new ObservableCollection<Service.Application.Dto.Students.StudentDto>();
        public const string StudentPropertyName = "StudentDtos";
        public ObservableCollection<Service.Application.Dto.Students.StudentDto> StudentDtos
        {
            get
            {
                return _studentDtos;
            }
            set
            {
                _studentDtos = value;
                RaisePropertyChanged(StudentPropertyName);
            }
        }

        private RelayCommand _testCommand;


        public RelayCommand TestCommand
        {
            get
            {
                return _testCommand ?? (_testCommand = new RelayCommand(TestFunc));
            }
        }

        private void TestFunc()
        {
            if (StudentService == null)
                throw new Exception("服务注入错误！");

            try
            {
                var tmp = StudentService.GetTest();
                Console.WriteLine(tmp.Length);
                StudentDtos = new ObservableCollection<Service.Application.Dto.Students.StudentDto>(tmp);
                Console.WriteLine($"转换后数目{StudentDtos.Count}");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}