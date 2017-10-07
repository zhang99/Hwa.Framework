using Microsoft.International.Converters.PinYinConverter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hwa.Framework.Util
{
    public class PinyinHelper
    {
        #region 获取汉字拼音/首字母

        /// <summary>
        /// 获取名称的助记码
        /// </summary>
        /// <param name="name"></param>
        /// <param name="length">助记码长度，为0表示获取所有长度</param>
        /// <returns></returns>
        public static string GetMnemonic(string name, int length)
        {
            length = length == 0 ? name.Length : length;
            var mnemonic = PinyinHelper.GetFirstPinyin(name);
            if (mnemonic != null && mnemonic.ByteLength() > length)//助记码最大长度
            {
                var maxLen = mnemonic.Length < length ? mnemonic.Length : length;
                mnemonic = mnemonic.Substring(0, maxLen).ToDBC();//全角转半角
                while (mnemonic.ByteLength() > length)//截取字节长度
                {
                    mnemonic = mnemonic.Remove(mnemonic.Length - 1);
                }
            }
            return mnemonic;
        }

        /// <summary>
        /// 获取汉字的首字母(部署时可能需要下载Microsoft Visual Studio International Pack 1.0 SR1，安装其中的CHSPinYinConv.msi
        /// 下载地址：http://www.microsoft.com/zh-cn/download/details.aspx?id=15251)
        /// 暂只考虑char值在19968-40869之间的汉字
        /// </summary>
        /// <param name="str">输入的汉字</param>
        /// <returns>汉字的拼音首字母</returns>
        public static string GetFirstPinyin(string str)
        {
            if (string.IsNullOrEmpty(str)) return str;
            return GetFirstPinyin(str, true);
        }

        /// <summary>
        /// 获取汉字的首字母(部署时可能需要下载Microsoft Visual Studio International Pack 1.0 SR1，安装其中的CHSPinYinConv.msi
        /// 下载地址：http://www.microsoft.com/zh-cn/download/details.aspx?id=15251)
        /// 暂只考虑char值在19968-40869之间的汉字
        /// </summary>
        /// <param name="str">输入的汉字</param>
        /// <param name="ingnoreSpecial">是否忽略特殊字符(只保留汉字、字母、数字、下划线)</param>
        /// <returns>汉字的拼音首字母</returns>
        public static string GetFirstPinyin(string str, bool ingnoreSpecial)
        {
            string r = string.Empty;
            int index = 0;//标识当前字符在字符串中的位置
            foreach (char obj in str)
            {
                try
                {
                    var charValue = (int)obj;
                    if (charValue >= 19968 && charValue <= 40869)
                    {
                        ChineseChar chineseChar = new ChineseChar(obj);
                        string t = chineseChar.Pinyins[0].ToString();
                        if (chineseChar.PinyinCount > 1)//多音字处理
                        {
                            var p = GetPolyphonePinyin(obj, str, index);
                            if (!string.IsNullOrEmpty(p)) t = p;
                        }
                        r += t.Substring(0, 1);
                    }
                    else if (ingnoreSpecial && (
                        (charValue >= 65 && charValue <= 90)        //大写字母
                        || (charValue >= 97 && charValue <= 122)    //小写字母
                        || (charValue >= 48 && charValue <= 57)     //数字0-9
                        || charValue == 95                          //下划线
                        ))
                    {
                        r += obj.ToString();
                    }
                    else if (ingnoreSpecial)
                    {
                        index++;
                        continue;
                    }
                    else
                        r += obj.ToString();
                }
                catch
                {
                    var p = GetUnknownPinyin(obj);
                    if (!string.IsNullOrEmpty(p) && p.Length > 0)
                        r += p.Substring(0, 1);
                }
                index++;
            }
            return r.ToUpper();
        }

        /// <summary> 
        /// 获取汉字的拼音(部署时可能需要下载Microsoft Visual Studio International Pack 1.0 SR1，安装其中的CHSPinYinConv.msi
        /// 下载地址：http://www.microsoft.com/zh-cn/download/details.aspx?id=15251)
        /// 暂只考虑char值在19968-40869之间的汉字
        /// <param name="str">汉字</param> 
        /// <returns>全拼</returns> 
        public static string GetPinyin(string str)
        {
            string r = string.Empty;
            int index = 0;//标识当前字符在字符串中的位置
            foreach (char obj in str)
            {
                try
                {
                    ChineseChar chineseChar = new ChineseChar(obj);
                    string t = chineseChar.Pinyins[0].ToString();
                    if (chineseChar.PinyinCount > 1)//多音字处理
                    {
                        var p = GetPolyphonePinyin(obj, str, index);
                        if (!string.IsNullOrEmpty(p)) t = p;
                    }
                    r += t.Substring(0, t.Length - 1);
                }
                catch
                {
                    var p = GetUnknownPinyin(obj);
                    if (!string.IsNullOrEmpty(p) && p.Length > 0)
                        r += p.Substring(0, 1);
                }
                index++;
            }
            return r;
        }

        const string _unknownArray = "丒 乄 乤 乥 乧 乫 乬 乭 乮 乯 乲 乶 乷 乺 乻 乼 乽 亪 伈 侤 俧 兺 凧 凩 凪 剦 匁 匇 匴 厼 叾 呠 哘 哛 唜 唞 唟 啂 営 喸 嗭 噛 噺 嚒 嚰 囕 囖 圷 圸 垪 垰 垳 埖 塀 塩 塰 墸 壊 壭 壱 売 夞 妵 婲 嬢 嬶 孻 対 専 岃 岼 岾 峅 嵶 巪 巼 幉 広 廃 廤 弐 弾 彁 応 怺 怾 恷 悩 愥 戦 扖 扥 扽 抙 抜 掵 掻 摂 敨 敻 旀 旕 昻 曢 朑 朩 朰 杁 杤 杦 枠 枩 柡 栃 栄 栍 桛 桜 桟 梺 梻 椙 椚 椛 椡 椦 椧 楾 楿 榁 榊 榋 槈 槗 槝 樮 樰 橲 橳 橴 橻 櫉 櫦 欕 欟 歚 歯 歳 毮 気 氞 浌 涜 渇 渋 渓 溌 澚 濏 濹 灐 炞 烪 焔 熋 燵 爳 犞 犠 猠 獣 珱 琑 瓧 瓩 瓰 瓱 瓲 瓸 瓼 甅 甼 畑 畓 畩 畳 疂 癦 癪 癷 発 硳 硴 礖 穃 穒 穝 笂 笹 笽 篒 簓 簗 簯 簱 籂 籎 籏 籖 籡 粀 粁 粂 粌 粏 粐 粫 粭 糀 糓 紏 絵 続 綛 緕 縅 縇 縨 繍 繧 繷 缼 羪 羺 翸 耨 聁 聢 聣 肀 肏 脳 脵 腉 膤 臓 舗 艝 苆 荘 莻 菐 萙 蒅 蒊 蓙 蓜 蔶 蘣 蘰 虄 虲 蛍 蜶 蝿 螧 蟵 袮 袰 裃 裄 褄 褜 褝 襙 襨 覅 訳 読 誮 譨 譳 讝 蹹 躮 躵 躾 軅 軈 転 軣 轌 辷 辺 迲 逤 逧 遖 郉 酛 醗 釈 釻 鈨 鈬 銰 鋲 錺 錻 鎒 鐞 鐢 閊 閕 閪 闏 陥 険 隲 霯 霻 靍 靎 靏 鞆 鞐 鞥 顕 颪 餎 饂 饹 駅 駲 験 騨 魞 魸 魹 鮴 鯐 鯑 鯱 鯲 鯳 鰚 鰰 鱛 鱜 鱫 鳰 鴫 鵆 鵇 鵈 鵥 鶍 鶎 鶑 鶫 麿 黁 黈 鼡";
        const string _unknownPinyinArray = "CHOU WU XIA HOL DOU JIA JU SHI MAO HU CAL FU SHA SUO YU ZHU ZHE YE XIN TA ZHI BUN ZHENG MU ZHI YAN MANGMI YI SUAN KEUM DUG PEN XING PPUN MAS TEO KEOS NOU YING PHOS CIS YAO XIN ME ME RAM LO XIA SHAN BING KA HANG HUA PING YAN HAI ZHU HUAI SAN YI MAI OES TOU HUA NIANG BI NAI DUI ZHUAN YEN PING ZHAN BIAN RUO JU PHAS DIE GUANG FEI KOS ER DAN GE YING YONG KI XIAO NAO YING ZHAN RU DEN DEN POU BA MING SAO ZHE TOU XIONG MYEO EOS ANG UU TI PIN TEUL RU WAN JIU ZUI SONG YONG LI RONG SAENG KASEI YING ZHAN XIA FO CHANG MEN HUA DAO QUAN MYEONG QUAN XIANG SHI SHEN CHU NOU QIAO DAO YAN XUE XI SHENG ZI CHU CHU QING YAN GUANG SHAN CHI SUI SHA QI BIN PEOL DU KE SE XI PO AO SE ME YING BIAN UU YAN NAI DA HAN QIAO XI CEON SHOU YING SUO SHIWA QIANWA FENWA MAOWA WA BAIWA LIWA LIWA DING TIAN TAP YI DIE DIE YI JI BO FA CEOK HUA YU RONG KWEOK ZUI WAN XIAO MIN SHI DIAO LIANG QI QI SHI YI QI QIAN GIE ZHANG QIAN ZHAI YIN TAI HU ER HE HUA GU TOU GUI XU REN QI WEI SEON HUANG XIU YUN NONG QI YANG NOU PEN NOU UU DING NI YU CAO NAO GU NAI XUE ZANG PU XUE QIE ZHUANG NEUS PU ZHEN RAN HUA ZUO PEI ZE TOU MAN SAL XIA YING SUO YING QI CHU MI BO KA XING QI PAO DAN CAO TEA FIAO YI DU HUA NOU NOU ZHAN TA FEN REN MEI YAN YING ZHUAN HONG XUE YI FAN QU SUO GU NAN GENG YUAN FA SHI QIU YUAN DUO NGAI BING FANG WU NOU NOU FAN SHAN XIA SEO PHDENG XIAN XIAN ZHI TENG FENG HE HE HE BING QIA ENG XIAN GUA LE WEN LE YI ZHOU YAN TUO BA PIAN MO XIU ZOU XI HU YU DI XUAN SHEN ZENG XIANG AI RU TIAN HENG NIAN E PAN YI ZUN YING DONG MO NUN TOU SHU";
        /// <summary>
        /// 获取未能识别的364个汉字的拼音
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        private static string GetUnknownPinyin(char c)
        {
            if (_unknownArray.IndexOf(c) >= 0)
            {
                var unknownStrs = _unknownArray.Split(' ');
                var unknownPinyinStrs = _unknownPinyinArray.Split(' ');
                int index = 0;
                index = unknownStrs.Select(f => new { Index = index++, CharStr = f }).Where(f => f.CharStr == c.ToString()).Select(f => f.Index).FirstOrDefault();

                return unknownPinyinStrs[index];
            }

            return string.Empty;
        }

        #region 多音字词库
        /// <summary>
        /// 多音字词库(TODO：暂只处理部分通过ChineseChar转化后出过问题的)
        /// </summary>
        static Dictionary<string, string> _polyphoneDictionary = null;
        static Dictionary<string, string> PolyphoneDictionary
        {
            get
            {
                if (_polyphoneDictionary == null)
                {
                    _polyphoneDictionary = new Dictionary<string, string>();
                    _polyphoneDictionary.Add("红", "红|HONG2");
                    _polyphoneDictionary.Add("紅", "紅|HONG2");
                    _polyphoneDictionary.Add("女红", "红|GONG1");
                    _polyphoneDictionary.Add("合", "合|HE2");//GE3
                    _polyphoneDictionary.Add("盛", "盛|SHENG4");//CHENG2
                    _polyphoneDictionary.Add("单", "单|DAN1");//CHAN2,SHAN4
                    _polyphoneDictionary.Add("纤", "纤|XIAN1");
                    _polyphoneDictionary.Add("纤夫", "纤|QIAN4");
                    _polyphoneDictionary.Add("无", "无|WU2");//MO2
                    _polyphoneDictionary.Add("奇", "奇|QI2");//JI1
                    _polyphoneDictionary.Add("强", "强|QIANG2");
                    _polyphoneDictionary.Add("倔强", "强|JIANG4");
                    _polyphoneDictionary.Add("粘", "粘|ZHAN1");//NIAN2,NIAN5
                    _polyphoneDictionary.Add("万", "万|WAN4");//MO4,WAN5
                    _polyphoneDictionary.Add("弹", "弹|TAN2");
                    _polyphoneDictionary.Add("弹弓", "弹|DAN4");
                    _polyphoneDictionary.Add("乐", "乐|LE4");
                    _polyphoneDictionary.Add("音乐", "乐|YUE4");
                    _polyphoneDictionary.Add("樂", "樂|LE4");
                    _polyphoneDictionary.Add("音樂", "樂|YUE4");
                    _polyphoneDictionary.Add("咖", "咖|KA1");
                    _polyphoneDictionary.Add("咖喱", "咖|GA1");
                    _polyphoneDictionary.Add("阿", "阿|A1");
                    _polyphoneDictionary.Add("阿胶", "阿|E1");
                    _polyphoneDictionary.Add("塔", "塔|TA3");//DA5
                    _polyphoneDictionary.Add("调", "调|TIAO2");//DIAO4,DIAO5
                    _polyphoneDictionary.Add("行", "行|XING2");
                    _polyphoneDictionary.Add("银行", "行|HANG2");
                    _polyphoneDictionary.Add("行业", "行|HANG2");
                    _polyphoneDictionary.Add("排行", "行|HANG2");
                    _polyphoneDictionary.Add("折", "折|ZHE2");//SHE2
                    _polyphoneDictionary.Add("戏", "戏|XI4");//HU1
                    _polyphoneDictionary.Add("重", "重|CHONG2");
                    _polyphoneDictionary.Add("重口味", "重|ZHONG4");
                    _polyphoneDictionary.Add("超重", "重|ZHONG4");
                    _polyphoneDictionary.Add("重量", "重|ZHONG4");
                    _polyphoneDictionary.Add("重油污", "重|ZHONG4");
                    _polyphoneDictionary.Add("体重", "重|ZHONG4");
                    _polyphoneDictionary.Add("传", "传|CHUAN2");
                    _polyphoneDictionary.Add("传记", "传|ZHUAN4");
                    _polyphoneDictionary.Add("提", "提|TI2");//DI1
                    _polyphoneDictionary.Add("适", "适|SHI4");//KUO4
                    _polyphoneDictionary.Add("匙", "匙|SHI5");//CHI2
                    _polyphoneDictionary.Add("兹", "兹|ZI1");//CI2,CI5
                    _polyphoneDictionary.Add("鸟", "鸟|NIAO3");//DIAO3
                    _polyphoneDictionary.Add("叶", "叶|YE4");//XIE2
                    _polyphoneDictionary.Add("夹", "夹|JIA2");//GA1,JIA1
                    _polyphoneDictionary.Add("夾", "夾|JIA2");//GA1,JIA1
                    _polyphoneDictionary.Add("圈", "圈|QUAN1");//JUAN1
                    _polyphoneDictionary.Add("颈", "颈|JING3");//GENG3
                    _polyphoneDictionary.Add("系", "系|XI4");//JI4
                    _polyphoneDictionary.Add("蜗", "蜗|WO1");//LUO2
                    _polyphoneDictionary.Add("扒", "扒|PA2");
                    _polyphoneDictionary.Add("扒皮", "扒|BA1");
                    _polyphoneDictionary.Add("参", "参|SHEN1");
                    _polyphoneDictionary.Add("参差", "参|CEN1");
                    _polyphoneDictionary.Add("参观", "参|CAN1");
                    _polyphoneDictionary.Add("虾", "虾|XIA1");//HA2
                    _polyphoneDictionary.Add("洋", "洋|YANG2");//XIANG2,YANG1
                    _polyphoneDictionary.Add("石", "石|SHI2");//DAN4
                    _polyphoneDictionary.Add("茄", "茄|QIE2");
                    _polyphoneDictionary.Add("雪茄", "茄|JIA1");
                    _polyphoneDictionary.Add("藏", "藏|CANG2");
                    _polyphoneDictionary.Add("西藏", "藏|ZANG4");
                    _polyphoneDictionary.Add("藏耗牛", "藏|ZANG4");
                    _polyphoneDictionary.Add("藏式", "藏|ZANG4");
                    _polyphoneDictionary.Add("藏青", "藏|ZANG4");
                    _polyphoneDictionary.Add("藏蓝", "藏|ZANG4");
                    _polyphoneDictionary.Add("藏极", "藏|ZANG4");
                    _polyphoneDictionary.Add("藏泉", "藏|ZANG4");
                    _polyphoneDictionary.Add("藏红花", "藏|ZANG4");
                    _polyphoneDictionary.Add("屏", "屏|PING2");//BING1,BING3
                    _polyphoneDictionary.Add("家", "家|JIA1");//GU1,JIE5
                    _polyphoneDictionary.Add("楂", "楂|ZHA1");//CHA2
                    _polyphoneDictionary.Add("靓", "靓|LIANG4");//JING4
                    _polyphoneDictionary.Add("长", "长|CHANG2");
                    _polyphoneDictionary.Add("成长", "长|ZHANG3");
                    _polyphoneDictionary.Add("船长", "长|ZHANG3");
                    _polyphoneDictionary.Add("长辈", "长|ZHANG3");
                    _polyphoneDictionary.Add("长大", "长|ZHANG3");
                    _polyphoneDictionary.Add("长官", "长|ZHANG3");
                    _polyphoneDictionary.Add("长进", "长|ZHANG3");
                    _polyphoneDictionary.Add("长膘", "长|ZHANG3");
                    _polyphoneDictionary.Add("广", "广|GUANG3");//AN1
                    _polyphoneDictionary.Add("廣", "廣|GUANG3");//AN1
                    _polyphoneDictionary.Add("选", "选|XUAN3");//SHUA1,SUAN4
                    _polyphoneDictionary.Add("呵", "呵|HE1");//A1,KE1
                    _polyphoneDictionary.Add("降", "降|JIANG4");
                    _polyphoneDictionary.Add("投降", "降|XIANG2");
                    _polyphoneDictionary.Add("降魔", "降|XIANG2");
                    _polyphoneDictionary.Add("齐", "齐|QI2");//JI4
                    _polyphoneDictionary.Add("唔", "唔|WU2");//EN2
                    _polyphoneDictionary.Add("便", "便|BIAN4");
                    _polyphoneDictionary.Add("便宜", "便|PIAN2");
                    _polyphoneDictionary.Add("脯", "脯|PU3");
                    _polyphoneDictionary.Add("果脯", "脯|FU3");
                    _polyphoneDictionary.Add("许", "许|XU3");//HU3
                    _polyphoneDictionary.Add("蕴", "蕴|YUN4");//WEN1
                    _polyphoneDictionary.Add("粥", "粥|ZHOU1");//YU1
                    _polyphoneDictionary.Add("镶", "镶|XIANG1");//RANG2
                    _polyphoneDictionary.Add("汤", "汤|TANG1");//SHANG1
                    _polyphoneDictionary.Add("湯", "湯|TANG1");//SHANG1
                    _polyphoneDictionary.Add("咀", "咀|ZUI3");
                    _polyphoneDictionary.Add("咀嚼", "咀|JU3");
                    _polyphoneDictionary.Add("其", "其|QI2");//JI1
                    _polyphoneDictionary.Add("会", "会|HUI4");
                    _polyphoneDictionary.Add("会稽", "会|KUAI4");
                    _polyphoneDictionary.Add("会计", "会|KUAI4");
                    _polyphoneDictionary.Add("咳", "咳|KE2");//HAI1
                    _polyphoneDictionary.Add("厂", "厂|CHANG3");//AN1
                    _polyphoneDictionary.Add("浅", "浅|QIAN3");//JIAN1
                    _polyphoneDictionary.Add("页", "页|YE4");//XIE2
                    _polyphoneDictionary.Add("哆", "哆|DUO1");//CHI3
                    _polyphoneDictionary.Add("泊", "泊|BO2");
                    _polyphoneDictionary.Add("水泊", "泊|PO1");
                    _polyphoneDictionary.Add("芥", "芥|JIE4");//GAI4
                    _polyphoneDictionary.Add("秘", "秘|MI4");//BI4
                    _polyphoneDictionary.Add("种", "种|ZHONG3");//CHONG2
                    _polyphoneDictionary.Add("哦", "哦|O2");//E2
                    _polyphoneDictionary.Add("刨", "刨|PAO2");//BAO4
                    _polyphoneDictionary.Add("發", "發|FA1");//BO1
                    _polyphoneDictionary.Add("么", "么|ME5");
                    _polyphoneDictionary.Add("么妹", "么|YAO1");
                    _polyphoneDictionary.Add("骑", "骑|QI2");//JI4
                    _polyphoneDictionary.Add("組", "組|ZU3");//QU1
                    _polyphoneDictionary.Add("綉", "綉|XIU4");//TOU4
                    _polyphoneDictionary.Add("铛", "铛|DANG1");//CHENG1
                    _polyphoneDictionary.Add("筴", "筴|JIA1");//CE4
                    _polyphoneDictionary.Add("厦", "厦|XIA4");
                    _polyphoneDictionary.Add("大厦", "厦|SHA4");
                    _polyphoneDictionary.Add("哑", "哑|YA2");//E4,YAI1
                    _polyphoneDictionary.Add("炮", "炮|PAO4");//BAO1,PAO2
                    _polyphoneDictionary.Add("宿", "宿|SU4");
                    _polyphoneDictionary.Add("星宿", "宿|XIU3");
                    _polyphoneDictionary.Add("期", "期|QI1");//JI1
                    _polyphoneDictionary.Add("巷", "巷|XIANG4");//HANG4
                    _polyphoneDictionary.Add("龈", "龈|YIN2");//KEN3
                    _polyphoneDictionary.Add("沱", "沱|TUO2");//DUO4
                    _polyphoneDictionary.Add("献", "献|XIAN4");//SUO1
                    _polyphoneDictionary.Add("吓", "吓|XIA4");//HE4
                    _polyphoneDictionary.Add("招", "招|ZHAO1");//QIAO2
                    _polyphoneDictionary.Add("校", "校|XIAO4");
                    _polyphoneDictionary.Add("校对", "校|JIAO4");
                    _polyphoneDictionary.Add("屹", "屹|YI4");//GE1
                    _polyphoneDictionary.Add("给", "给|GEI3");
                    _polyphoneDictionary.Add("给水", "给|JI3");
                    _polyphoneDictionary.Add("孖", "孖|ZI2");//MA1
                    _polyphoneDictionary.Add("纶", "纶|LUN2");//GUAN1
                    _polyphoneDictionary.Add("呷", "呷|XIA1");//GA1
                    _polyphoneDictionary.Add("轧", "轧|ZHA2");//GA2,YA4,YAI4
                    _polyphoneDictionary.Add("涌", "涌|YONG3");//CHONG1
                    _polyphoneDictionary.Add("乾", "乾|QIAN2");//GAN1
                    _polyphoneDictionary.Add("且", "且|QIE3");//JU1
                    _polyphoneDictionary.Add("伽", "伽|JIA1");//GA1
                    _polyphoneDictionary.Add("区", "区|QU1");//OU1
                    _polyphoneDictionary.Add("溃", "溃|KUI4");//HUI4
                    _polyphoneDictionary.Add("拂", "拂|FU2");//BI4
                    _polyphoneDictionary.Add("俞", "俞|YU2");//SHU4
                    _polyphoneDictionary.Add("選", "選|XUAN3");//SHUA1,SUAN4
                    _polyphoneDictionary.Add("蛤", "蛤|HA2");
                    _polyphoneDictionary.Add("蛤蜊", "蛤|GE2");
                    _polyphoneDictionary.Add("蛤蚧", "蛤|GE2");
                    _polyphoneDictionary.Add("椎", "椎|ZHUI1");//CHUI2
                    _polyphoneDictionary.Add("蝦", "蝦|XIA1");//HA2
                    _polyphoneDictionary.Add("骆", "骆|LUO4");//JIA4
                    _polyphoneDictionary.Add("锗", "锗|ZHE3");//DU3
                    _polyphoneDictionary.Add("遗", "遗|YI2");//WEI4
                    _polyphoneDictionary.Add("強", "強|QIANG2");//JIANG4
                    _polyphoneDictionary.Add("腌", "腌|YAN1");//A1
                    _polyphoneDictionary.Add("刹", "刹|SHA1");//CHA4
                    _polyphoneDictionary.Add("裳", "裳|SHANG5");//CHANG2
                    _polyphoneDictionary.Add("荨", "荨|XUN2");//QIAN2
                    _polyphoneDictionary.Add("衰", "衰|SHUAI1");//CUI1
                    _polyphoneDictionary.Add("耙", "耙|PA2");//BA4
                    _polyphoneDictionary.Add("泌", "泌|MI4");//BI4
                    _polyphoneDictionary.Add("曾", "曾|CENG2");
                    _polyphoneDictionary.Add("曾师傅", "曾|ZENG1");
                    _polyphoneDictionary.Add("曾大哥", "曾|ZENG1");
                    _polyphoneDictionary.Add("曾大妈", "曾|ZENG1");
                    _polyphoneDictionary.Add("曾大姐", "曾|ZENG1");
                    _polyphoneDictionary.Add("曾老师", "曾|ZENG1");
                    _polyphoneDictionary.Add("荠", "荠|JI4");//CI2,QI2
                    _polyphoneDictionary.Add("喳", "喳|ZHA1");//CHA1
                    _polyphoneDictionary.Add("涡", "涡|WO1");//GUO1
                    _polyphoneDictionary.Add("卒", "卒|ZU2");//CU4
                    _polyphoneDictionary.Add("嚣", "嚣|XIAO1");//AO2
                    _polyphoneDictionary.Add("邹", "邹|ZOU1");//JU4
                    _polyphoneDictionary.Add("偲", "偲|SI1");//CAI1

                }
                return _polyphoneDictionary;
            }
        }
        #endregion

        /// <summary>
        /// 获取多音字常用读音或在词组中的读音(TODO：暂无更好方案，先整理一部分)
        /// </summary>
        /// <param name="c">多音字符</param>
        /// <param name="str">所在词组或字符串</param>
        /// <param name="index">c在str中所处的索引位置</param>
        /// <returns></returns>
        public static string GetPolyphonePinyin(char c, string str, int index)
        {
            var polyphoneDictionary = PolyphoneDictionary;
            if (polyphoneDictionary == null || string.IsNullOrEmpty(str)) return "";
            var cStr = c.ToString();
            var temp = str;
            var temp1 = string.Empty;

            //1.先匹配全字
            if (polyphoneDictionary.ContainsKey(temp))
            {
                temp1 = polyphoneDictionary[temp];
                if (temp1.Split('|')[0] == cStr) return temp1.Split('|')[1];
            }
            //2.从当前c所在的位置，从两头往中间截取，逐个匹配
            while (index >= 0)
            {
                temp = str;
                if (polyphoneDictionary.ContainsKey(temp))
                {
                    temp1 = polyphoneDictionary[temp];
                    if (temp1.Split('|')[0] == cStr) return temp1.Split('|')[1];
                }
                while (index < temp.Length - 1)
                {
                    temp = temp.Substring(0, temp.Length - 1);
                    if (polyphoneDictionary.ContainsKey(temp))
                    {
                        temp1 = polyphoneDictionary[temp];
                        if (temp1.Split('|')[0] == cStr) return temp1.Split('|')[1];
                    }
                }
                if (index > 0) str = str.Substring(1);
                index--;
            }
            return "";
        }

        #endregion
    }
}
