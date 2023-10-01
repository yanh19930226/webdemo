namespace webdemo.Models.Dto.Category
{
    public class CreateCategoryVo
    {
        /// <summary>
        /// 
        /// </summary>
        public int ServiceId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int ParentId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public long Id { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [System.ComponentModel.DataAnnotations.Required(ErrorMessage = "{0} 必须填写")]
        public string CategoryName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int Sort { get; set; }
    }
}
