using cocos2d;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TD.Classes.Grounds;
using TD.Classes.Maps;

namespace TD.Classes.Roles
{
    public class Role : GroundBase
    {
        #region 字段

        private Direction direction = Direction.RIGHT;
        private CCLayer layer;
        private Map map;
        private GroundBase OringinGround = null;
        private LinkedList<GroundBase> pathNodeList = new LinkedList<GroundBase>();
        #endregion

        #region 属性

        public Direction Direction
        {
            get
            {
                return direction;
            }
            set
            {
                direction = value;
            }
        }

        #endregion

        #region 构造方法

        public Role(string roleImgPath, CCPoint position, CCLayer layer, Map map)
        {
            this.map = map;
            this.layer = layer;
            Col = (int)position.x;
            Row = (int)position.y;
            if (!string.IsNullOrEmpty(roleImgPath))
            {
                Sprite = CCSprite.spriteWithFile(roleImgPath);
                Sprite.position = new CCPoint(position.x * AppContext.MAP_WIDTH + 15, position.y * AppContext.MAP_WIDTH + 15);

                layer.addChild(Sprite);
            }

            OringinGround = map.GroundList[Row * Map.Width + Col];

            GetPathNodeList();
        }

        #endregion

        #region 重写方法

        #endregion

        #region 方法

        public void Move()
        {
            LinkedListNode<GroundBase> currentNode = pathNodeList.First;
            LinkedListNode<GroundBase> nextNode = currentNode.Next;

            if (nextNode == null)
            {
                AppContext.Arrived = true;
                return;
            }

            switch (GetDirection(currentNode, nextNode))
            {
                case Direction.UP:
                    {
                        CCMoveBy moveBy = CCMoveBy.actionWithDuration(AppContext.UPDATE_INTERVAL, new CCPoint(0, AppContext.MAP_WIDTH));
                        Sprite.runAction(moveBy);
                        Row++;
                        break;
                    }
                case Direction.DOWN:
                    {
                        CCMoveBy moveBy = CCMoveBy.actionWithDuration(AppContext.UPDATE_INTERVAL, new CCPoint(0, -AppContext.MAP_WIDTH));
                        Sprite.runAction(moveBy);
                        Row--;
                        break;
                    }
                case Direction.LEFT:
                    {
                        CCMoveBy moveBy = CCMoveBy.actionWithDuration(AppContext.UPDATE_INTERVAL, new CCPoint(-AppContext.MAP_WIDTH, 0));
                        Sprite.runAction(moveBy);
                        Col--;
                        break;
                    }
                case Direction.RIGHT:
                    {
                        CCMoveBy moveBy = CCMoveBy.actionWithDuration(AppContext.UPDATE_INTERVAL, new CCPoint(AppContext.MAP_WIDTH, 0));
                        Sprite.runAction(moveBy);
                        Col++;
                        break;
                    }
            }
            pathNodeList.RemoveFirst();
        }

        public void GetPathNodeList()
        {
            if (!AppContext.Arrived)
            {
                pathNodeList.Clear();
                OringinGround = map.GroundList[Row * Map.Width + Col];
                AStart();
            }
        }

        private void AStart()
        {
            GroundBase upGround = null;
            GroundBase downGround = null;
            GroundBase leftGround = null;
            GroundBase rightGround = null;

            List<GroundBase> openList = new List<GroundBase>();
            List<GroundBase> closeList = new List<GroundBase>();

            GroundBase shortestNode = map.GroundList[Row * Map.Width + Col];

            // 寻找到终点
            openList.Add(shortestNode);
            while (shortestNode != null && GetDistance(new Point(shortestNode.Row, shortestNode.Col), map.Destination) != 0)
            {
                if (shortestNode.Row < Map.Height - 1)
                {
                    upGround = map.GroundList[(shortestNode.Row + 1) * Map.Width + shortestNode.Col];
                    if (upGround.GroundType != 3)
                    {
                        if (!openList.Contains(upGround) && !closeList.Contains(upGround))
                        {
                            upGround.CostH = GetDistance(new Point(upGround.Row, upGround.Col), map.Destination);
                            upGround.CostF = upGround.CostG + upGround.CostH;
                            upGround.PreGround = shortestNode;

                            openList.Add(upGround);
                        }
                    }
                }
                if (shortestNode.Row > 0)
                {
                    downGround = map.GroundList[(shortestNode.Row - 1) * Map.Width + shortestNode.Col];
                    if (downGround.GroundType != 3)
                    {
                        if (!openList.Contains(downGround) && !closeList.Contains(downGround))
                        {
                            downGround.CostH = GetDistance(new Point(downGround.Row, downGround.Col), map.Destination);
                            downGround.CostF = downGround.CostG + downGround.CostH;
                            downGround.PreGround = shortestNode;

                            openList.Add(downGround);
                        }
                    }
                }
                if (shortestNode.Col > 0)
                {
                    leftGround = map.GroundList[shortestNode.Row * Map.Width + shortestNode.Col - 1];
                    if (leftGround.GroundType != 3)
                    {
                        if (!openList.Contains(leftGround) && !closeList.Contains(leftGround))
                        {
                            leftGround.CostH = GetDistance(new Point(leftGround.Row, leftGround.Col), map.Destination);
                            leftGround.CostF = leftGround.CostG + leftGround.CostH;
                            leftGround.PreGround = shortestNode;

                            openList.Add(leftGround);
                        }
                    }
                }
                if (shortestNode.Col < Map.Width - 1)
                {
                    rightGround = map.GroundList[shortestNode.Row * Map.Width + shortestNode.Col + 1];
                    if (rightGround.GroundType != 3)
                    {
                        if (!openList.Contains(rightGround) && !closeList.Contains(rightGround))
                        {
                            rightGround.CostH = GetDistance(new Point(rightGround.Row, rightGround.Col), map.Destination);
                            rightGround.CostF = rightGround.CostG + rightGround.CostH;
                            rightGround.PreGround = shortestNode;

                            openList.Add(rightGround);
                        }
                    }
                }
                shortestNode = GetShortestFNode(openList);

                if (shortestNode != null)
                {
                    // 将openList中F值最小的节点从openList移除加入closeList
                    openList.Remove(shortestNode);
                    closeList.Add(shortestNode);
                }
            }

            // 回退到起点,计算出路径
            LinkedListNode<GroundBase> endNode = new LinkedListNode<GroundBase>(closeList.Last());
            pathNodeList.AddLast(endNode);
            while (endNode.Value.PreGround != OringinGround)
            {
                var temp = new LinkedListNode<GroundBase>(endNode.Value.PreGround);
                pathNodeList.AddBefore(endNode, temp);
                endNode = temp;
            }
            pathNodeList.AddFirst(OringinGround);
        }

        /// <summary>
        /// 两点之间距离
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns></returns>
        private int GetDistance(Point from, Point to)
        {
            return Math.Abs(to.X - from.X) + Math.Abs(to.Y - from.Y);
        }

        /// <summary>
        /// 在OpenList中寻找F值最小的节点
        /// </summary>
        /// <param name="openNodeList"></param>
        /// <returns></returns>
        private GroundBase GetShortestFNode(List<GroundBase> openNodeList)
        {
            int shortestDistance = int.MaxValue;
            GroundBase shortestNode = null;

            if (openNodeList != null)
            {
                foreach (var item in openNodeList)
                {
                    int tempDistance = GetDistance(new Point(item.Row, item.Col), map.Destination);
                    if (tempDistance < shortestDistance)
                    {
                        shortestDistance = tempDistance;
                        shortestNode = item;
                    }
                }
            }
            return shortestNode;
        }

        /// <summary>
        /// 获取要行走的方向
        /// </summary>
        /// <param name="currentNode"></param>
        /// <param name="nextNode"></param>
        /// <returns></returns>
        private Direction GetDirection(LinkedListNode<GroundBase> currentNode, LinkedListNode<GroundBase> nextNode)
        {
            if (currentNode.Value.Row != nextNode.Value.Row)
            {
                if (currentNode.Value.Row < nextNode.Value.Row)
                    return Direction.UP;
                else
                    return Direction.DOWN;
            }
            else
                if (currentNode.Value.Col != nextNode.Value.Col)
                {
                    if (currentNode.Value.Col > nextNode.Value.Col)
                        return Direction.LEFT;
                    else
                        return Direction.RIGHT;
                }
            return Classes.Direction.UP;
        }
        #endregion
    }
}
