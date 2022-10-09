using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Ball : MonoBehaviour
{
    private BlockController _blockController;

    public void OnCreate(BlockController blockController)
    {
        _blockController = blockController;
    }

    public void OnCall(Vector3 targetPos)
    {
        JumpToTarget(targetPos);
    }
    
    private void JumpToTarget(Vector3 targetPos)
    {
        Vector3[] path =
        {
            targetPos, 
            new Vector3(transform.localPosition.x * 1.33f, -3.5f, -0.4f)
        };
        
        /*transform.DOLocalJump(targetPos, 1f, 1, 1.25f)
            .OnComplete(() => transform.DOLocalJump(, 1f, 1, 1.25f))};*/

        transform.DOLocalPath(path, 2f, PathType.CatmullRom);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PixelBlock>())
        {
            var indices = other.name.Split("-");
            var posX = int.Parse(indices[1]);
            var posY = int.Parse(indices[0]);

            if (_blockController.IsExistInMap(posY, posX) && !_blockController.GetDidFinish())
            {
                _blockController.RemoveFromMap(posX, posY);
            }
        }
    }
}
