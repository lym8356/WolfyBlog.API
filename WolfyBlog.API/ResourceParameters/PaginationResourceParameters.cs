namespace WolfyBlog.API.ResourceParameters
{
	public class PaginationResourceParameters
	{
        private int _pageNumber = 1;
        public int PageNumber
        {
            get
            {
                return _pageNumber;
            }
            set
            {
                if (value >= 1)
                {
                    _pageNumber = value;
                }
            }
        }

        //private int _pageSize = 5;
        private int? _pageSize = null;
        const int maxPageSize = 10;
        public int? PageSize
        {
            get
            {
                return _pageSize;
            }
            set
            {
                if (value == null || value >= 1)
                {
                    _pageSize = (value > maxPageSize) ? maxPageSize : value;
                }
            }
        }

    }
}

