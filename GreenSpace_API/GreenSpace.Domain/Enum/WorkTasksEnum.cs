namespace GreenSpace.Domain.Enum
{
    public enum WorkTasksEnum
    {
        ConsultingAndSket = 0,
        DoneConsulting = 1,
        Design = 2,
        DoneDesign = 3,
        DesignDetail = 4,
        DoneDesignDetail = 5,
        Completed = 6,
        Pending = 7,                 // chờ xử lí
        Installing = 8,               // đang hỗ trợ lắp đặt
        DoneInstalling = 9,              // hoàn thành lắp đặt
        ReInstall = 10,                     // làm lại


    }
    public enum  ContructTaksEnum
    {
        Pending = 0,                 // chờ xử lí
        Installing = 1,               // đang hỗ trợ lắp đặt
        DoneInstalling = 2,              // hoàn thành lắp đặt
        ReInstall = 3,                     // làm lại
        Completed = 4,                   
    }
}
