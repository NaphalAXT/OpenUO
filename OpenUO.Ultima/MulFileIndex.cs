#region License Header
// /***************************************************************************
//  *   Copyright (c) 2011 OpenUO Software Team.
//  *   All Right Reserved.
//  *
//  *   MulFileIndex.cs
//  *
//  *   This program is free software; you can redistribute it and/or modify
//  *   it under the terms of the GNU General Public License as published by
//  *   the Free Software Foundation; either version 3 of the License, or
//  *   (at your option) any later version.
//  ***************************************************************************/
#endregion

#region References
using System.Collections.Generic;
using System.IO;
#endregion

namespace OpenUO.Ultima
{
	public class MulFileIndex : FileIndexBase
	{
		private readonly string _IndexPath;

		public MulFileIndex(string idxFile, string mulFile)
			: base(mulFile)
		{
			_IndexPath = idxFile;
		}

		public override bool FilesExist { get { return File.Exists(_IndexPath) && base.FilesExist; } }

		protected override FileIndexEntry[] ReadEntries()
		{
			var entries = new List<FileIndexEntry>();

			int length = (int)((new FileInfo(_IndexPath).Length / 3) / 4);

			using (FileStream index = new FileStream(_IndexPath, FileMode.Open, FileAccess.Read, FileShare.Read))
			{
				BinaryReader bin = new BinaryReader(index);

				int count = (int)(index.Length / 12);

				for (int i = 0; i < count && i < length; ++i)
				{
					FileIndexEntry entry = new FileIndexEntry {
						Lookup = bin.ReadInt32(),
						Length = bin.ReadInt32(),
						Extra = bin.ReadInt32()
					};

					entries.Add(entry);
				}

				for (int i = count; i < length; ++i)
				{
					FileIndexEntry entry = new FileIndexEntry {
						Lookup = -1,
						Length = -1,
						Extra = -1,
					};

					entries.Add(entry);
				}
			}

			return entries.ToArray();
		}
	}
}