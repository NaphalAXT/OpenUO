﻿#region License Header
// /***************************************************************************
//  *   Copyright (c) 2011 OpenUO Software Team.
//  *   All Right Reserved.
//  *
//  *   DirectedAcyclicGraph.cs
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
#endregion

namespace OpenUO.Core.Patterns
{
	public sealed class DirectedAcyclicGraph<T>
	{
		private readonly Dictionary<string, GraphNode<T>> _nodes = new Dictionary<string, GraphNode<T>>();

		public void AddNode(GraphNode<T> dependencyGraphNode)
		{
			_nodes.Add(dependencyGraphNode.Identifier, dependencyGraphNode);
		}

		public GraphNode<T> GetNode(string identity)
		{
			if (_nodes.ContainsKey(identity))
			{
				return _nodes[identity];
			}

			throw (new Exception(string.Format("Cannot find node '{0}'", identity)));
		}

		public List<GraphNode<T>> ComputeDependencyOrderedList()
		{
			var results = new List<GraphNode<T>>();

			var nodeList = new List<Vertex>();
			var nodeHashtable = new Dictionary<GraphNode<T>, Vertex>();

			foreach (var idv in _nodes.Values)
			{
				Vertex vertex = new Vertex(idv);

				nodeList.Add(vertex);
				nodeHashtable.Add(idv, vertex);
			}

			foreach (Vertex vertex in nodeList)
			{
				vertex.EstablishChildRelationships(nodeHashtable);
			}

			bool isResortNeeded = true;

			while (nodeList.Count > 0)
			{
				if (isResortNeeded)
				{
					nodeList.Sort(DefaultVertexComparer.Instance);
				}

				Vertex nextVertex = nodeList[0];
				nodeList.RemoveAt(0);
				results.Add(nextVertex.Underlying);

				isResortNeeded = (nextVertex.Underlying.DependsOn.Count > 0);

				if (nextVertex.Order != 0)
				{
					throw new Exception(
						string.Format(
							"Node '{0}' cannot directly or indirectly depend on node '{0}' because that would result in a cyclic relationship",
							nextVertex.Identifier));
				}

				nextVertex.DecrementParentsChildCount(nodeHashtable);
			}

			return results;
		}

		private sealed class DefaultVertexComparer : IComparer<Vertex>
		{
			public static readonly DefaultVertexComparer Instance = new DefaultVertexComparer();

			public int Compare(Vertex x, Vertex y)
			{
				if (x.Order < y.Order)
				{
					return -1;
				}

				if (x.Order > y.Order)
				{
					return 1;
				}

				if (x.Order > 0)
				{
					return 0;
				}

				try
				{
					return x.Underlying.CompareTo(y.Underlying);
				}
				catch
				{
					return 0;
				}
			}
		}

		private sealed class Vertex
		{
			internal readonly GraphNode<T> Underlying;
			internal int Order;

			internal Vertex(GraphNode<T> payload)
			{
				Underlying = payload;
				Order = 0;
			}

			public string Identifier { get { return Underlying.Identifier; } }

			internal void EstablishChildRelationships(IDictionary<GraphNode<T>, Vertex> otherVertices)
			{
				foreach (var idv in Underlying.DependsOn)
				{
					Vertex v = otherVertices[idv];
					v.Order++;
				}
			}

			internal void DecrementParentsChildCount(IDictionary<GraphNode<T>, Vertex> otherVertices)
			{
				foreach (var idv in Underlying.DependsOn)
				{
					Vertex v = otherVertices[idv];
					v.Order--;
				}
			}
		}
	}
}