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

namespace WindowsFormsApplication1
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
	
	
	[global::System.Data.Linq.Mapping.DatabaseAttribute(Name="db_data")]
	public partial class linqtosqlClassDataContext : System.Data.Linq.DataContext
	{
		
		private static System.Data.Linq.Mapping.MappingSource mappingSource = new AttributeMappingSource();
		
    #region 可扩展性方法定义
    partial void OnCreated();
    partial void Insertmlmoban(mlmoban instance);
    partial void Updatemlmoban(mlmoban instance);
    partial void Deletemlmoban(mlmoban instance);
    #endregion
		
		public linqtosqlClassDataContext() : 
				base(global::WindowsFormsApplication1.Properties.Settings.Default.db_dataConnectionString, mappingSource)
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
		
		public System.Data.Linq.Table<mlmoban> mlmoban
		{
			get
			{
				return this.GetTable<mlmoban>();
			}
		}
	}
	
	[global::System.Data.Linq.Mapping.TableAttribute(Name="dbo.mlmoban")]
	public partial class mlmoban : INotifyPropertyChanging, INotifyPropertyChanged
	{
		
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
		private string _模板编号;
		
		private string _命令类型;
		
		private string _命令内容;
		
    #region 可扩展性方法定义
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
    partial void On模板编号Changing(string value);
    partial void On模板编号Changed();
    partial void On命令类型Changing(string value);
    partial void On命令类型Changed();
    partial void On命令内容Changing(string value);
    partial void On命令内容Changed();
    #endregion
		
		public mlmoban()
		{
			OnCreated();
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_模板编号", DbType="NChar(10) NOT NULL", CanBeNull=false, IsPrimaryKey=true)]
		public string 模板编号
		{
			get
			{
				return this._模板编号;
			}
			set
			{
				if ((this._模板编号 != value))
				{
					this.On模板编号Changing(value);
					this.SendPropertyChanging();
					this._模板编号 = value;
					this.SendPropertyChanged("模板编号");
					this.On模板编号Changed();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_命令类型", DbType="NChar(100)")]
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
					this.On命令类型Changing(value);
					this.SendPropertyChanging();
					this._命令类型 = value;
					this.SendPropertyChanged("命令类型");
					this.On命令类型Changed();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_命令内容", DbType="NChar(200)")]
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
					this.On命令内容Changing(value);
					this.SendPropertyChanging();
					this._命令内容 = value;
					this.SendPropertyChanged("命令内容");
					this.On命令内容Changed();
				}
			}
		}
		
		public event PropertyChangingEventHandler PropertyChanging;
		
		public event PropertyChangedEventHandler PropertyChanged;
		
		protected virtual void SendPropertyChanging()
		{
			if ((this.PropertyChanging != null))
			{
				this.PropertyChanging(this, emptyChangingEventArgs);
			}
		}
		
		protected virtual void SendPropertyChanged(String propertyName)
		{
			if ((this.PropertyChanged != null))
			{
				this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}
	}
}
#pragma warning restore 1591