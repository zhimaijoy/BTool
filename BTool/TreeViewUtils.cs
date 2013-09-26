﻿using System;
using System.Windows.Forms;

public class TreeViewUtils
{
    public const string moduleName = "TreeViewUtils";
    private static bool recursiveDestroy;

    public void ClearSelectedNode(TreeView treeView)
    {
        treeView.SelectedNode = null;
    }

    private bool RecursiveTextSearchAndDestroy(TreeNode treeNode, string target)
    {
        bool flag = false;
        if (treeNode.Text == target)
        {
            flag = true;
            recursiveDestroy = true;
            return flag;
        }
        foreach (TreeNode node in treeNode.Nodes)
        {
            if (node != null)
            {
                flag = this.RecursiveTextSearchAndDestroy(node, target);
                if (flag && recursiveDestroy)
                {
                    treeNode.Remove();
                    recursiveDestroy = false;
                    return flag;
                }
            }
        }
        return flag;
    }

    public void RemoveNameFromTree(TreeView treeView, string name)
    {
        foreach (TreeNode node in treeView.Nodes)
        {
            if (node.Name == name)
            {
                node.Remove();
            }
        }
    }

    public void RemoveTextFromTree(TreeView treeView, string text)
    {
        foreach (TreeNode node in treeView.Nodes)
        {
            if ((node != null) && (node.Text == text))
            {
                node.Remove();
            }
        }
    }

    public bool TextViewSearchAndDestroy(TreeView treeView, string target)
    {
        bool flag = false;
        recursiveDestroy = false;
        foreach (TreeNode node in treeView.Nodes)
        {
            flag = this.RecursiveTextSearchAndDestroy(node, target);
            if (flag)
            {
                return flag;
            }
        }
        return flag;
    }

    public bool TreeNodeTextSearchAndDestroy(TreeNode treeNode, string target)
    {
        recursiveDestroy = false;
        return this.RecursiveTextSearchAndDestroy(treeNode, target);
    }
}
