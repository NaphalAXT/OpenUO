﻿#region License Header
// /***************************************************************************
//  *   Copyright (c) 2011 OpenUO Software Team.
//  *   All Right Reserved.
//  *
//  *   UltimaSDKBitmapModule.cs
//  *
//  *   This program is free software; you can redistribute it and/or modify
//  *   it under the terms of the GNU General Public License as published by
//  *   the Free Software Foundation; either version 3 of the License, or
//  *   (at your option) any later version.
//  ***************************************************************************/
#endregion

#region References
using System.Drawing;

using OpenUO.Core.Patterns;
using OpenUO.Ultima.Adapters;
using OpenUO.Ultima.Windows.Forms.Adapters;
#endregion

namespace OpenUO.Ultima.Windows.Forms
{
	public class UltimaSDKBitmapModule : IModule
	{
		public string Name { get { return "OpenUO Ultima SDK - Bitmap Module"; } }

		public void OnLoad(Container container)
		{
			container.Register<IArtworkStorageAdapter<Bitmap>, ArtworkBitmapAdapter>();
			container.Register<IAnimationStorageAdapter<Bitmap>, AnimationBitmapStorageAdapter>();
			container.Register<IASCIIFontStorageAdapter<Bitmap>, ASCIIFontBitmapAdapter>();
			container.Register<IGumpStorageAdapter<Bitmap>, GumpBitmapAdapter>();
			container.Register<ITexmapStorageAdapter<Bitmap>, TexmapBitmapAdapter>();
			container.Register<IUnicodeFontStorageAdapter<Bitmap>, UnicodeFontBitmapAdapter>();
		}

		public void OnUnload(Container container)
		{ }
	}
}