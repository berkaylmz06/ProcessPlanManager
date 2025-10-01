using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CEKA_APP.Helper
{
    public static class TreeNodeKoleksiyonuGenisletmeleri
    {
        public static TreeNode[] FindByFullPath(this TreeNodeCollection nodes, string fullPath)
        {
            if (string.IsNullOrEmpty(fullPath))
                return new TreeNode[0];

            foreach (TreeNode node in nodes)
            {
                var found = FindNodeByFullPath(node, fullPath);
                if (found != null)
                    return new[] { found };
            }
            return new TreeNode[0];
        }

        private static TreeNode FindNodeByFullPath(TreeNode node, string fullPath)
        {
            if (node.FullPath == fullPath)
                return node;

            foreach (TreeNode child in node.Nodes)
            {
                var found = FindNodeByFullPath(child, fullPath);
                if (found != null)
                    return found;
            }
            return null;
        }
    }
}
