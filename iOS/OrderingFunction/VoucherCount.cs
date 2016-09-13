using System;
namespace HookMeUP.iOS
{
	class VoucherCount
	{

		bool IsVoucherDepleted
		{
			get;
			set;
		}
		bool IsVoucherNegative
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
				if (IsSelected) --Voucher;
				if (IsDeselected) ++Voucher;

			}
			else if (Voucher == 0) IsVoucherDepleted = true;
			else
			{
				IsVoucherNegative = true;

			}
		}

		public int GetVoucher()
		{
			return Voucher;
		}
	}
}

