using UnityEngine;
using System.Collections;

namespace EditorWinEx.Internal
{
    public struct EWRectangle
    {

        public bool useSimplePercentage;

        public float x;
        public float y;
        public float z;
        public float w;

        public bool anchorLeft;
        public bool anchorTop;
        public bool anchorRight;
        public bool anchorBottom;

        public EWRectangle(float x, float y, float z, float w, bool anchorLeft, bool anchorRight, bool anchorTop,
            bool anchorBottom)
        {
            useSimplePercentage = false;
            this.x = x;
            this.y = y;
            this.z = z;
            this.w = w;
            this.anchorLeft = anchorLeft;
            this.anchorRight = anchorRight;
            this.anchorTop = anchorTop;
            this.anchorBottom = anchorBottom;
        }

        public EWRectangle(float x, float y, float width, float height)
        {
            useSimplePercentage = true;
            this.x = x;
            this.y = y;
            this.z = width;
            this.w = height;
            this.anchorLeft = false;
            this.anchorRight = false;
            this.anchorTop = false;
            this.anchorBottom = false;
        }

        public Rect GetRect(Rect rect)
        {
            if (useSimplePercentage)
            {
                return new Rect(rect.x + rect.width*x, rect.y + rect.height*y, rect.width*z, rect.height*w);
            }
            else
            {
                Rect result = default(Rect);
                if (anchorLeft && anchorRight)
                {
                    result.x = rect.x + x;
                    result.width = rect.width - x - z;
                }else if (anchorLeft)
                {
                    result.x = rect.x + x;
                    result.width = z;
                }else if (anchorRight)
                {
                    result.x = rect.x + rect.width - x - z;
                    result.width = z;
                }
                else
                {
                    result.x = rect.x + rect.width/2 + x - z/2;
                    result.width = z;
                }
                if (anchorTop && anchorBottom)
                {
                    result.y = rect.y + y;
                    result.height = rect.height - y - w;
                }else if (anchorTop)
                {
                    result.y = rect.y + y;
                    result.height = w;
                }else if (anchorBottom)
                {
                    result.y = rect.y + rect.height - y - w;
                    result.height = w;
                }
                else
                {
                    result.y = rect.y + rect.height/2 + y - w/2;
                    result.height = w;
                }
                return result;
            }
        }
    }
}