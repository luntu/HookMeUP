using System;
using System.Diagnostics;

namespace HookMeUP.iOS
{
	class VoucherCount
	{

		public bool IsVoucherDepleted
		{
			get;
			private set;
		}
		public bool IsVoucherNegative
		{
			get;
			private set;
		}

		bool HasTag
		{ 
			get;
			set;
		}

		public int Voucher
		{
			get;
			set;
		}

		public double Price
		{
			get;
			set;
		}

		public bool IsSelected
		{
			get;
			set;
		}

		public bool IsDeselected
		{
			get;
			set;
		}

		public void VoucherChange()
		{
			if (Voucher > 0)
			{
				IsVoucherNegative = false;
				IsVoucherDepleted = false;
				if (IsSelected) --Voucher;
				if (IsDeselected) ++Voucher;

			}
			//else if (Voucher == 0) IsVoucherDepleted = true;
			else
			{
				IsVoucherNegative = true;
				IsVoucherDepleted = false;
			}
		}

		public int GetVoucher()
		{
			return Voucher;
		}
	}
}

