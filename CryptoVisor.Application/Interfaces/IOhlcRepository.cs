﻿using CryptoVisor.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoVisor.Application.Interfaces
{
	public interface IOhlcRepository
	{
		Task SaveListAsync(IEnumerable<OhclCoinHistory> dataList);
		Task<IEnumerable<OhclCoinHistory>> GetDataFromPeriod(DateTime firstDate, DateTime lastDate, ECoinType eCoinType);
		Task<bool> VerifyIfExistsCoinOnPeriod(DateTime firstDate, DateTime lastDate, ECoinType eCoinType);
		Task TruncateTable();
	}
}
