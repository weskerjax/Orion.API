using System;

namespace Orion.API.Models
{
	/// <summary>Assembly Meta 資訊</summary>
	public class AssemblyMeta
    {
        /// <summary></summary>
        public string Title { get; internal set; }
        
        /// <summary></summary>
        public string Description { get; internal set; }
        
        /// <summary></summary>
        public string Configuration { get; internal set; }
        
        /// <summary></summary>
        public string Company { get; internal set; }
        
        /// <summary></summary>
        public string Product { get; internal set; }
        
        /// <summary></summary>
        public string Copyright { get; internal set; }
        
        /// <summary></summary>
        public string Trademark { get; internal set; }
        
        /// <summary></summary>
        public string Culture { get; internal set; }
        
        /// <summary></summary>
        public string Guid { get; internal set; }
        
        /// <summary></summary>
        public Version Version { get; internal set; }

        /// <summary></summary>
        public string FileVersion { get; internal set; }
    }
}
