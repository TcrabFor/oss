//------------------------------------------------------------------------------
// <auto-generated>
//     此代码已从模板生成。
//
//     手动更改此文件可能导致应用程序出现意外的行为。
//     如果重新生成代码，将覆盖对此文件的手动更改。
// </auto-generated>
//------------------------------------------------------------------------------

namespace Oss.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class SystemResourceModule
    {
        public System.Guid Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string Url { get; set; }
        public int Type { get; set; }
        public Nullable<System.Guid> ParentId { get; set; }
        public Nullable<int> OrderNo { get; set; }
    }
}