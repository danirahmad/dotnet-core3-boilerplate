// ------------------------------------------------------------------------------
// <auto-generated>
//    Generated by avrogen, version 1.7.7.5
//    Changes to this file may cause incorrect behavior and will be lost if code
//    is regenerated
// </auto-generated>
// ------------------------------------------------------------------------------
namespace Moonlay.Topics.Customers
{
	using System;
	using System.Collections.Generic;
	using System.Text;
	using global::Avro;
	using global::Avro.Specific;
	
	public partial class UpdateCustomerTopic : ISpecificRecord
	{
		public static Schema _SCHEMA = Schema.Parse("{\"type\":\"record\",\"name\":\"UpdateCustomerTopic\",\"namespace\":\"Moonlay.Topics.Custome" +
				"rs\",\"fields\":[{\"name\":\"CustomerId\",\"type\":\"string\"},{\"name\":\"FirstName\",\"type\":\"" +
				"string\"},{\"name\":\"LastName\",\"type\":\"string\"}]}");
		private string _CustomerId;
		private string _FirstName;
		private string _LastName;
		public virtual Schema Schema
		{
			get
			{
				return UpdateCustomerTopic._SCHEMA;
			}
		}
		public string CustomerId
		{
			get
			{
				return this._CustomerId;
			}
			set
			{
				this._CustomerId = value;
			}
		}
		public string FirstName
		{
			get
			{
				return this._FirstName;
			}
			set
			{
				this._FirstName = value;
			}
		}
		public string LastName
		{
			get
			{
				return this._LastName;
			}
			set
			{
				this._LastName = value;
			}
		}
		public virtual object Get(int fieldPos)
		{
			switch (fieldPos)
			{
			case 0: return this.CustomerId;
			case 1: return this.FirstName;
			case 2: return this.LastName;
			default: throw new AvroRuntimeException("Bad index " + fieldPos + " in Get()");
			};
		}
		public virtual void Put(int fieldPos, object fieldValue)
		{
			switch (fieldPos)
			{
			case 0: this.CustomerId = (System.String)fieldValue; break;
			case 1: this.FirstName = (System.String)fieldValue; break;
			case 2: this.LastName = (System.String)fieldValue; break;
			default: throw new AvroRuntimeException("Bad index " + fieldPos + " in Put()");
			};
		}
	}
}
