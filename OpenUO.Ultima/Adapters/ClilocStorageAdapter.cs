#region License Header
// /***************************************************************************
//  *   Copyright (c) 2011 OpenUO Software Team.
//  *   All Right Reserved.
//  *
//  *   ClilocStorageAdapter.cs
//  *
//  *   This program is free software; you can redistribute it and/or modify
//  *   it under the terms of the GNU General Public License as published by
//  *   the Free Software Foundation; either version 3 of the License, or
//  *   (at your option) any later version.
//  ***************************************************************************/
#endregion

#region References
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
#endregion

namespace OpenUO.Ultima.Adapters
{
	public class ClientLocalizationStorageAdapter : StorageAdapterBase, IClilocStorageAdapter<ClilocInfo>
	{
		private readonly Dictionary<ClientLocalizationLanguage, ClientLocalizations> _Tables =
			new Dictionary<ClientLocalizationLanguage, ClientLocalizations>
			{
				{ClientLocalizationLanguage.ENU, new ClientLocalizations()},
				{ClientLocalizationLanguage.DEU, new ClientLocalizations()},
				{ClientLocalizationLanguage.ESP, new ClientLocalizations()},
				{ClientLocalizationLanguage.FRA, new ClientLocalizations()},
				{ClientLocalizationLanguage.JPN, new ClientLocalizations()},
				{ClientLocalizationLanguage.KOR, new ClientLocalizations()},
				{ClientLocalizationLanguage.CHT, new ClientLocalizations()}
			};

		public Dictionary<ClientLocalizationLanguage, ClientLocalizations> Tables { get { return _Tables; } }

		public override int Length
		{
			get
			{
				if (!IsInitialized)
				{
					Initialize();
				}

				return _Tables[0].Count;
			}
		}

		public override void Initialize()
		{
			base.Initialize();

			var tables = new List<ClientLocalizations>(_Tables.Values);
			bool loaded = tables.TrueForAll(t => t.Loaded);

			if (loaded || (Install == null || String.IsNullOrWhiteSpace(Install.Directory)))
			{
				return;
			}

			Parallel.ForEach(
				_Tables,
				kvp =>
				{
					if (kvp.Value.Loaded)
					{
						return;
					}

					string stub = Path.Combine(Install.Directory, "/Cliloc." + kvp.Key.ToString().ToLower());

					if (File.Exists(stub))
					{
						kvp.Value.Load(new FileInfo(stub));
					}
				});
		}

		public ClilocInfo GetCliloc(ClientLocalizationLanguage lng, int index)
		{
			if (_Tables.ContainsKey(lng) && _Tables[lng] != null)
			{
				return _Tables[lng].Lookup(index);
			}

			return null;
		}

		public string GetRawString(ClientLocalizationLanguage lng, int index)
		{
			if (_Tables.ContainsKey(lng) && _Tables[lng] != null && !_Tables[lng].IsNullOrEmpty(index))
			{
				return _Tables[lng][index].Text;
			}

			return String.Empty;
		}

		public string GetString(ClientLocalizationLanguage lng, int index, string args)
		{
			ClilocInfo info = GetCliloc(lng, index);

			if (info == null)
			{
				return String.Empty;
			}

			return info.ToString(args);
		}

		public string GetString(ClientLocalizationLanguage lng, int index, params string[] args)
		{
			ClilocInfo info = GetCliloc(lng, index);

			if (info == null)
			{
				return String.Empty;
			}

			return info.ToString(args);
		}
	}
}