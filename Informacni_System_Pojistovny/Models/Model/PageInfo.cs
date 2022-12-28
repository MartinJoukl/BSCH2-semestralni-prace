namespace Informacni_System_Pojistovny.Models.Model
{
    public class PageInfo
    {
        private int pageSize = 20;
        private int pageIndex = 0;
        public int PageSize
        {
            get
            {
                return pageSize;
            }
            set
            {
                if (value >= 1)
                {
                    pageSize = value;
                }
            }
        }
        public int PageIndex
        {
            get
            {
                return pageIndex;
            }
            set
            {
                if (value >= 0)
                {
                    pageIndex = value;
                }
            }
        }
    }
}
