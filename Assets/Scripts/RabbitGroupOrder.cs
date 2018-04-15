using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enums;

public class RabbitGroupOrder{
    List<RabbitGroup> groupOrder;
    int index;
    public RabbitGroupOrder(int size = 0){
        groupOrder = new List<RabbitGroup>(0);
        index = -1;
    }
    public RabbitGroupOrder Set(RabbitGroup group, int n = 1){
        for (int i = 0; i < n; i++){
            groupOrder.Add(group);
        }
        return this;
    }
    public int GetSize(){
        return groupOrder.Count;
    }
    public RabbitGroup GetNext(){
        index++;
        if (index == groupOrder.Count) {
            index = 0;
        }
        return groupOrder[index];
    }

    public static RabbitGroupOrder GetOrderData(int stageIndex){
        var result = new RabbitGroupOrder();
        switch(stageIndex){
            case 1:
                result.Set(RabbitGroup.LeisurelyMore);
                break;
            case 2:
                result.Set(RabbitGroup.LeisurelyMore, 4)
                    .Set(RabbitGroup.LeisurelySingle);
                break;
            case 3:
                result.Set(RabbitGroup.LeisurelyMore)
                    .Set(RabbitGroup.LeisurelyDouble)
                    .Set(RabbitGroup.LeisurelySingle);
                break;
            case 4:
                result.Set(RabbitGroup.LeisurelyMore, 9)
                    .Set(RabbitGroup.VIP);
                break;
            case 5:
                result.Set(RabbitGroup.LeisurelyMore, 3)
                    .Set(RabbitGroup.HastyMore);
                break;
            case 6:
                result.Set(RabbitGroup.HastyMore);
                break;
            case 7:
                result.Set(RabbitGroup.LeisurelyDouble, 3)
                    .Set(RabbitGroup.NormalDouble);
                break;
            case 8:
                result.Set(RabbitGroup.LeisurelyMore, 2)
                    .Set(RabbitGroup.LeisurelyDouble)
                    .Set(RabbitGroup.LeisurelyMore, 2)
                    .Set(RabbitGroup.LeisurelySingle);
                break;
            case 9:
                result.Set(RabbitGroup.HastySingle);
                break;
            case 10:
                result.Set(RabbitGroup.LeisurelyMore, 2)
                    .Set(RabbitGroup.HastyMore)
                    .Set(RabbitGroup.LeisurelyMore, 2)
                    .Set(RabbitGroup.HastySingle)
                    .Set(RabbitGroup.LeisurelyMore, 2)
                    .Set(RabbitGroup.VIP);
                break;
            case 11:
                result.Set(RabbitGroup.LeisurelyMore);
                break;
            case 12:
                result.Set(RabbitGroup.NormalMore, 2)
                    .Set(RabbitGroup.NormalDouble);
                break;
            case 13:
                result.Set(RabbitGroup.LeisurelyMore, 3)
                    .Set(RabbitGroup.NormalMore, 3)
                    .Set(RabbitGroup.LeisurelyMore, 3)
                    .Set(RabbitGroup.FullLevel);
                break;
            case 14:
                result.Set(RabbitGroup.LeisurelyMore, 5)
                    .Set(RabbitGroup.LeisurelyDouble)
                    .Set(RabbitGroup.LeisurelyMore, 2)
                    .Set(RabbitGroup.NormalSingle)
                    .Set(RabbitGroup.LeisurelyMore, 2)
                    .Set(RabbitGroup.VIP, 3)
                    .Set(RabbitGroup.LeisurelyMore, 2)
                    .Set(RabbitGroup.NormalSingle)
                    .Set(RabbitGroup.LeisurelyMore, 2);
                break;
            case 15:
                result.Set(RabbitGroup.LeisurelyMore, 3)
                    .Set(RabbitGroup.NormalDouble, 2)
                    .Set(RabbitGroup.VIP);
                break;
            case 16:
                result.Set(RabbitGroup.HastyMore)
                    .Set(RabbitGroup.HastyDouble)
                    .Set(RabbitGroup.HastySingle);
                break;
            case 17:
                result.Set(RabbitGroup.LeisurelyMore, 2)
                    .Set(RabbitGroup.VIP)
                    .Set(RabbitGroup.LeisurelyDouble)
                    .Set(RabbitGroup.LeisurelyMore)
                    .Set(RabbitGroup.LeisurelySingle)
                    .Set(RabbitGroup.HastyMore)
                    .Set(RabbitGroup.LeisurelyMore)
                    .Set(RabbitGroup.NormalSingle)
                    .Set(RabbitGroup.HastyDouble);
                break;
            case 18:
                result.Set(RabbitGroup.NormalMore)
                    .Set(RabbitGroup.NormalDouble)
                    .Set(RabbitGroup.HastyMore)
                    .Set(RabbitGroup.LeisurelyDouble)
                    .Set(RabbitGroup.LeisurelyMore)
                    .Set(RabbitGroup.NormalSingle);
                break;
            case 19:
                result.Set(RabbitGroup.LeisurelyMore,2)
                    .Set(RabbitGroup.LeisurelyDouble)
                    .Set(RabbitGroup.LeisurelyMore)
                    .Set(RabbitGroup.LeisurelySingle);
                break;
            case 20:
                result.Set(RabbitGroup.LeisurelyMore,2)
                    .Set(RabbitGroup.LeisurelyDouble,2)
                    .Set(RabbitGroup.LeisurelyMore)
                    .Set(RabbitGroup.LeisurelySingle)
                    .Set(RabbitGroup.LeisurelyMore,4)
                    .Set(RabbitGroup.LeisurelyDouble,2)
                    .Set(RabbitGroup.NormalMore,3)
                    .Set(RabbitGroup.NormalDouble,2)
                    .Set(RabbitGroup.NormalMore,3)
                    .Set(RabbitGroup.NormalSingle)
                    .Set(RabbitGroup.NormalMore)
                    .Set(RabbitGroup.HastyMore,2)
                    .Set(RabbitGroup.HastyDouble)
                    .Set(RabbitGroup.HastySingle)
                    .Set(RabbitGroup.VIP)
                    .Set(RabbitGroup.FullLevel);
                break;
            
            default:
                break;
        }
        return result;
    }
}