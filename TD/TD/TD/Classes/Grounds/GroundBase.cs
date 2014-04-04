using cocos2d;

namespace TD.Classes.Grounds
{
    public class GroundBase
    {
        #region 字段

        #endregion

        #region 属性

        public CCSprite Sprite;

        public GroundBase PreGround = null;

        /// <summary>
        /// 是否访问过
        /// </summary>
        public bool Visited = false;

        /// <summary>
        /// 行
        /// </summary>
        public int Row;

        /// <summary>
        /// 列
        /// </summary>
        public int Col;

        public int CostH;

        public int CostG = 1;

        public int CostF;

        public int GroundType;

        #endregion

        #region 构造方法
        public GroundBase() { }

        public GroundBase(int typeId, CCPoint position, CCLayer layer)
        {
            this.GroundType = typeId;
            string imgPath = GetRoleImgPathByRoleTypeId(typeId);
            Row = (int)position.y;
            Col = (int)position.x;
            if (!string.IsNullOrEmpty(imgPath))
            {
                Sprite = CCSprite.spriteWithFile(imgPath);
                Sprite.position = new CCPoint(position.x * AppContext.MAP_WIDTH + 15, position.y * AppContext.MAP_WIDTH + 15);
                layer.addChild(Sprite);
            }
        }
        #endregion

        #region 重写方法

        #endregion

        #region 方法

        private string GetRoleImgPathByRoleTypeId(int id)
        {
            switch (id)
            {
                case 1:
                    {
                        return "imgs/g1";
                    }
                case 2:
                    {
                        return "imgs/g2";
                    }
                case 3:
                    {
                        return "imgs/g3";
                    }
                case 4:
                    {
                        return "imgs/g4";
                    }
                default:
                    {
                        return string.Empty;
                    }
            }
        }

        #endregion
    }
}
