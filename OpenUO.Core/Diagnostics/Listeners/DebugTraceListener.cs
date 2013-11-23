﻿#region License Header
// /***************************************************************************
//  *   Copyright (c) 2011 OpenUO Software Team.
//  *   All Right Reserved.
//  *
//  *   DebugTraceListener.cs
//  *
//  *   This program is free software; you can redistribute it and/or modify
//  *   it under the terms of the GNU General Public License as published by
//  *   the Free Software Foundation; either version 3 of the License, or
//  *   (at your option) any later version.
//  ***************************************************************************/
#endregion

#region References
using System.Diagnostics;
#endregion

namespace OpenUO.Core.Diagnostics
{
	public sealed class DebugTraceListener : TraceListener
	{
		protected override void OnTraceReceived(TraceMessage message)
		{
			Debug.WriteLine(message);
		}
	}
}