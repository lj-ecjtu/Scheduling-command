﻿#pragma warning disable 1591
//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由工具生成。
//     运行时版本:4.0.30319.17929
//
//     对此文件的更改可能会导致不正确的行为，并且如果
//     重新生成代码，这些更改将会丢失。
// </auto-generated>
//------------------------------------------------------------------------------

namespace NanJingNanZhan
{
	using System.Data.Linq;
	using System.Data.Linq.Mapping;
	using System.Data;
	using System.Collections.Generic;
	using System.Reflection;
	using System.Linq;
	using System.Linq.Expressions;
	using System.ComponentModel;
	using System;
	
	
	[global::System.Data.Linq.Mapping.DatabaseAttribute(Name="db_NanJingNanZhan")]
	public partial class linqtosqlClassDataContext : System.Data.Linq.DataContext
	{
		
		private static System.Data.Linq.Mapping.MappingSource mappingSource = new AttributeMappingSource();
		
    #region 可扩展性方法定义
    partial void OnCreated();
    #endregion
		
		public linqtosqlClassDataContext() : 
				base(global::NanJingNanZhan.Properties.Settings.Default.db_NanJingNanZhanConnectionString, mappingSource)
		{
			OnCreated();
		}
		
		public linqtosqlClassDataContext(string connection) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public linqtosqlClassDataContext(System.Data.IDbConnection connection) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public linqtosqlClassDataContext(string connection, System.Data.Linq.Mapping.MappingSource mappingSource) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public linqtosqlClassDataContext(System.Data.IDbConnection connection, System.Data.Linq.Mapping.MappingSource mappingSource) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public System.Data.Linq.Table<qianshouml> qianshouml
		{
			get
			{
				return this.GetTable<qianshouml>();
			}
		}
	}
	
	[global::System.Data.Linq.Mapping.TableAttribute(Name="dbo.qianshouml")]
	public partial class qianshouml
	{
		
		private string _命令编号;
		
		private string _命令类型;
		
		private string _命令内容;
		
		private string _是否签收;
		
		private string _签收人;
		
		private System.Nullable<System.DateTime> _签收时间;
		
		private string _签收结果;
		
		public qianshouml()
		{
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_命令编号", DbType="NChar(10)")]
		public string 命令编号
		{
			get
			{
				return this._命令编号;
			}
			set
			{
				if ((this._命令编号 != value))
				{
					this._命令编号 = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_命令类型", DbType="NChar(10)")]
		public string 命令类型
		{
			get
			{
				return this._命令类型;
			}
			set
			{
				if ((this._命令类型 != value))
				{
					this._命令类型 = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_命令内容", DbType="NChar(100)")]
		public string 命令内容
		{
			get
			{
				return this._命令内容;
			}
			set
			{
				if ((this._命令内容 != value))
				{
					this._命令内容 = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_是否签收", DbType="NChar(10)")]
		public string 是否签收
		{
			get
			{
				return this._是否签收;
			}
			set
			{
				if ((this._是否签收 != value))
				{
					this._是否签收 = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_签收人", DbType="NChar(10)")]
		public string 签收人
		{
			get
			{
				return this._签收人;
			}
			set
			{
				if ((this._签收人 != value))
				{
					this._签收人 = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_签收时间", DbType="DateTime")]
		public System.Nullable<System.DateTime> 签收时间
		{
			get
			{
				return this._签收时间;
			}
			set
			{
				if ((this._签收时间 != value))
				{
					this._签收时间 = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_签收结果", DbType="NChar(10)")]
		public string 签收结果
		{
			get
			{
				return this._签收结果;
			}
			set
			{
				if ((this._签收结果 != value))
				{
					this._签收结果 = value;
				}
			}
		}
	}
}
#pragma warning restore 1591