# HttpPrint

[TOC]



## 项目说明

- 本项目开发初衷：

  1.方便web端调用热敏打印机

  2.打印模板可配置

- 项目结构：
  1.设计器：SevenSoft.HttpPrint.Designer
  2.部署服务：SevenSoft.HttpPrint.Service
  3.通用工具类：SevenSoft.HttpPrint.Utils


## 接口文档

**请求URL：** 

- http://print.7soft.cn:666/print/

**请求方式：**

- POST 

**参数：**

| 参数名   | 类型                | 说明                                       |
| :------- | :------------------ | :----------------------------------------- |
| template | Template            | 打印模板（直接配置在数据库，前端无须处理） |
| copies   | short               | 打印份数                                   |
| values   | Map<string, string> | 替换值                                     |

**Template：**

| 参数名  | 类型            | 说明     |
| :------ | :-------------- | :------- |
| width   | int             | 打印模板 |
| height  | int             | 打印份数 |
| content | List<PrintItem> | 模板元素 |

**PrintItem：**

| 参数名    | 类型      | 说明                    |
| :-------- | :-------- | :---------------------- |
| PrtType   | string    | 打印类型                |
| PrtColor  | Color     | 打印颜色                |
| Start     | Point     | 起始位置                |
| End       | Point     | 结束位置                |
| Size      | Point     | 大小                    |
| Wigth     | int       | 宽                      |
| Height    | int       | 高                      |
| FontStyle | FontStyle | 字体样式  默认：Regular |
| Content   | string    | 内容                    |
| Column    | int       | 列数                    |
| Row       | int       | 行数                    |


 **请求示例**


``` json
{
    "template":{
        "width":300,
        "height":200,
        "content":[
            {
                "PrtType":"BarCode",
                "PrtColor":"Black",
                "Start":"100, 230",
                "End":"0, 0",
                "Size":0,
                "Wigth":250,
                "Height":50,
                "FontStyle":"Regular",
                "Content":"{OrderCode}",
                "Column":0,
                "Row":0
            }]
    },
    "copies":1,
    "values":{
        "OrderCode":"ABC123"
    }
}

```



## TodoList：

* [x ]通过HTTP调用打印机
* [x] 简易设计器
* [x] 支持二维码和条码打印
* [ ] 支持数据列表数据加载（如订单明细）
* [ ] 支持可变长度模板
* [ ] 可拖拽式设计器