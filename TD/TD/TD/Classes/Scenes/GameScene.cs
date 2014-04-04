using cocos2d;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using TD.Classes.Grounds;
using TD.Classes.Maps;
using TD.Classes.Roles;

namespace TD.Classes.Scenes
{
    public class GameScene : CCScene, ICCTargetedTouchDelegate
    {
        #region 字段

        private Game game;
        private CCSprite star;
        private Role role;
        private Map map;
        private CCLayer groundLayer = new CCLayer();
        private CCTexture2D wallTexture = CCTextureCache.sharedTextureCache().addImage("imgs/g1");
        #endregion

        #region 属性

        #endregion

        #region 构造方法

        public GameScene(Game game)
        {
            this.game = game;
            map = new Map(groundLayer);
            role = new Role("imgs/role1", new CCPoint(0, 0), groundLayer, map);
            this.addChild(groundLayer);
            this.schedule(update, AppContext.UPDATE_INTERVAL);
        }

        #endregion

        #region 重写方法

        public override void update(float dt)
        {
            base.update(dt);
            if (AppContext.Arrived)
            {
                this.unschedule(update);
            }
            role.Move();
        }

        public override void onEnter()
        {
            base.onEnter();
            CCTouchDispatcher.sharedDispatcher().addTargetedDelegate(this, 0, false);
        }

        public override void onExit()
        {
            base.onExit();
            CCTouchDispatcher.sharedDispatcher().removeDelegate(this);
        }

        public bool ccTouchBegan(CCTouch pTouch, CCEvent pEvent)
        {
            return true;
        }

        public void ccTouchCancelled(CCTouch pTouch, CCEvent pEvent)
        {
        }

        public void ccTouchEnded(CCTouch pTouch, CCEvent pEvent)
        {
            CCPoint point = pTouch.locationInView(0);
            CCPoint newPoint = CCDirector.sharedDirector().convertToGL(point);
            CCPoint rePoint = GetGroundPosition(newPoint);
            if ((int)rePoint.x < Map.Height &&
                (int)rePoint.y < Map.Width &&
                (role.Col != (int)rePoint.y || role.Row != (int)rePoint.x))
            {
                map.ChangeToWall((int)rePoint.x, (int)rePoint.y);
            }
            role.GetPathNodeList();
        }

        public void ccTouchMoved(CCTouch pTouch, CCEvent pEvent)
        {
        }
        #endregion

        #region 方法

        private void InitStar()
        {
            star = CCSprite.spriteWithFile("imgs/star");
            star.position = new CCPoint(CCDirector.sharedDirector().getWinSize().width / 2,
                CCDirector.sharedDirector().getWinSize().height / 2);
            this.addChild(star);
        }

        private CCPoint GetGroundPosition(CCPoint clickPosition)
        {
            int row = (int)Math.Floor(clickPosition.y / AppContext.MAP_WIDTH);
            int col = (int)Math.Floor(clickPosition.x / AppContext.MAP_WIDTH);
            return new CCPoint(row, col);
        }
        #endregion
    }
}
