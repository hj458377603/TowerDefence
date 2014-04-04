using cocos2d;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TD.Classes.Grounds;

namespace TD.Classes.Maps
{
    public class Map
    {
        #region 字段

        private CCLayer layer;

        #endregion

        #region 属性

        public const int Width = 26;
        public const int Height = 16;

        public int[,] Data ={
                            { 0, 0, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 
                            0, 0, 0, 3, 3, 3, 0, 0, 0, 0 },
                            { 3, 3, 0, 3, 3, 3, 0, 0, 0, 3, 0, 3, 3, 3, 3, 3, 
                            3, 0, 3, 3, 3, 3, 0, 0, 0, 0 },
                            { 0, 3, 3, 3, 0, 3, 3, 3, 0, 3, 0, 0, 3, 0, 0, 0, 
                            3, 0, 0, 3, 0, 3, 0, 0, 0, 0 },
                            { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3, 0, 3, 0, 0, 0, 
                            0, 0, 0, 3, 0, 3, 0, 0, 0, 0 },
                            { 0, 3, 3, 3, 3, 0, 0, 3, 3, 3, 0, 0, 3, 0, 0, 0, 
                            3, 0, 0, 3, 0, 3, 3, 0, 0, 0 },
                            { 0, 3, 0, 0, 0, 3, 3, 0, 0, 0, 0, 0, 3, 3, 0, 0, 
                            3, 0, 0, 3, 3, 0, 3, 0, 0, 0 },
                            { 0, 3, 0, 0, 0, 0, 0, 0, 3, 3, 3, 0, 0, 3, 0, 0, 
                            3, 0, 0, 3, 3, 0, 0, 3, 0, 0 },
                            { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3, 0, 3, 3, 3, 0, 
                            0, 0, 0, 3, 0, 0, 0, 3, 0, 0 },
                            { 3, 3, 0, 0, 3, 3, 0, 0, 0, 3, 3, 3, 3, 0, 0, 0, 
                            0, 0, 3, 3, 0, 0, 0, 3, 0, 0 },
                            { 0, 3, 3, 3, 0, 3, 3, 3, 3, 3, 0, 0, 0, 0, 3, 3, 
                            3, 3, 3, 0, 0, 0, 0, 3, 0, 0 },
                            { 0, 0, 0, 0, 0, 0, 0, 3, 3, 0, 0, 0, 0, 0, 0, 0, 
                            0, 0, 0, 0, 0, 0, 3, 3, 0, 0 },
                            { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 
                            0, 0, 0, 0, 3, 3, 3, 0, 0, 0 },
                            { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3, 3, 3, 0, 0, 0, 
                            0, 3, 3, 3, 3, 0, 0, 0, 0, 0 },
                            { 0, 0, 0, 3, 3, 3, 3, 3, 3, 3, 3, 0, 3, 3, 3, 3, 
                            3, 3, 0, 0, 0, 0, 0, 0, 0, 0 },
                            { 0, 0, 0, 3, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 
                            0, 0, 0, 0, 0, 0, 0, 0, 4, 0 },
                            { 0, 0, 0, 0, 0, 0, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 
                            3, 3, 0, 0, 0, 0, 0, 0, 0, 0 }
                            };
        public Point Destination = new Point(14, 24);
        public List<GroundBase> GroundList;

        #endregion

        #region 构造方法

        public Map(CCLayer layer)
        {
            this.layer = layer;
            GroundBase ground = null;
            GroundList = new List<GroundBase>();
            for (int row = 0; row < Height; row++)
            {
                for (int col = 0; col < Width; col++)
                {
                    ground = new GroundBase(Data[row, col], new CCPoint(col, row), layer);
                    if (ground.Sprite != null)
                    {
                        ground.Sprite.tag = row * Width + col;
                    }
                    GroundList.Add(ground);
                }
            }
        }

        #endregion

        #region 重写方法

        #endregion

        #region 方法

        public void ChangeToWall(int row, int col)
        {
            if (Data[row, col] != 3)
            {
                GroundList[row * Width + col] = new GroundBase(3, new CCPoint(col, row), layer);
                Data[row, col] = 3;
            }
            else
            {
                if (GroundList[row * Width + col].Sprite != null)
                {
                    GroundList[row * Width + col].Sprite.removeFromParentAndCleanup(true);
                }
                Data[row, col] = 0;
                GroundList[row * Width + col].GroundType = 0;
            }
        }

        #endregion
    }
}
