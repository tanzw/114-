using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace QueryPlatform.Code.Common
{
    //public class UserFonts : ConfigurationSection
    //{
    //    [ConfigurationProperty("UserFonts", IsRequired = true)]
    //    public FontElement UserFonts
    //    {
    //        get { return (FontElement)this["font"]; }
    //    }
    //}
    public class UserFontSection : ConfigurationSection	// 所有配置节点都要选择这个基类
    {
        private static readonly ConfigurationProperty s_property
            = new ConfigurationProperty(string.Empty, typeof(UserFontCollection), null,
                                            ConfigurationPropertyOptions.IsDefaultCollection);

       [ConfigurationProperty("", IsDefaultCollection = true)]     
        public UserFontCollection KeyValues
        {
            get
            {
                return (UserFontCollection)base[s_property];
            }
        }
    }


    [ConfigurationCollection(typeof(FontSetting))]
    public class UserFontCollection : ConfigurationElementCollection		// 自定义一个集合
    {
        // 基本上，所有的方法都只要简单地调用基类的实现就可以了。

        public UserFontCollection()
            : base(StringComparer.OrdinalIgnoreCase)	// 忽略大小写
        {
        }

        // 其实关键就是这个索引器。但它也是调用基类的实现，只是做下类型转就行了。
        new public FontSetting this[string name]
        {
            get
            {
                return (FontSetting)base.BaseGet(name);
            }
        }

        // 下面二个方法中抽象类中必须要实现的。
        protected override ConfigurationElement CreateNewElement()
        {
            return new FontSetting();
        }
        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((FontSetting)element).Key;
        }

        // 说明：如果不需要在代码中修改集合，可以不实现Add, Clear, Remove
        public void Add(FontSetting setting)
        {
            this.BaseAdd(setting);
        }
        public void Clear()
        {
            base.BaseClear();
        }
        public void Remove(string name)
        {
            base.BaseRemove(name);
        }
    }

    public class FontSetting : ConfigurationElement	// 集合中的每个元素
    {
        [ConfigurationProperty("Key", IsRequired = true)]
        public string Key
        {
            get { return this["Key"].ToString(); }
            set { this["Key"] = value; }
        }

        [ConfigurationProperty("Name", IsRequired = true)]
        public string Name
        {
            get { return this["Name"].ToString(); }
            set { this["Name"] = value; }
        }

        [ConfigurationProperty("Size", IsRequired = true)]
        public int Size
        {
            get { return Convert.ToInt32(this["Size"]); }
            set { this["Size"] = value; }
        }

        [ConfigurationProperty("Bold", IsRequired = true)]
        public int Bold
        {
            get { return Convert.ToInt32(this["Bold"]); }
            set { this["Bold"] = value; }
        }
    }   
}
