using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Globals
{
    /************************************** Prefabs **************************************/
    private const string PREFABS = "Prefabs/";
    public const string TUTORIALS_CONTAINER = PREFABS + "TutorialContatiner";
    public const string DOT = PREFABS + "Dot";

    public const string SAVED_GAME_IDS = "SavedGameIds";
    public const string MPL_GAME = "MPL_Game_";
    public const string TUTORIALS = "Tutorials";
    public const string LOGO = "Logo";
    public const string PREVIEW = "Preview";
    public const string LOGOTYPE = "LT";
    public const string COLLECTABLE = "Collectable";
    public const string ICON = "Icon";
    public const string TUTORIALTEXT = "TutorialTexts";

    public const string PREVIEW_ASSET_BUNDLE = "preview_assets";
    public const string PREVIEW_ASSETS_PATH = "Assets/Resources/";

    public static Vector2 PORTRAIT_RES = new Vector2(360, 640);
    public static Vector2 LANDSCAPE_RES = new Vector2(640, 360);

    //private const float SCORE_SYNC_INTERVAL = 0.05f;


    /************************************** Constants ************************************/












    public static readonly List<int> USER_IDS = new List<int>()
    {
        681268, 681269, 681270, 681271, 681272, 681273, 681274, 681276, 681277, 681278, 681279, 681280, 681281, 681283, 681284, 681285, 681286, 681287, 681288, 681289, 681290, 681291, 681292, 681293, 681295, 681297, 681298, 681299, 681300, 681301, 681302, 681303, 681308, 681309, 681310, 681311, 681312, 681313, 681314, 681315, 681316, 681317, 681318, 681319, 681320, 681321, 681322, 681323, 681324, 681325, 681326, 681327, 681328, 681329, 681330, 681331, 681332, 681333, 681334, 681335, 681336, 681337, 681338, 681339, 681340, 681341, 681342, 681343, 681344, 681345, 681346, 681347, 681348, 681349, 681350, 681352, 681353, 681354, 681355, 681356, 681357, 681358, 681359, 681360, 681361, 681362, 681363, 681364, 681365, 681366, 681367, 681368, 681369, 681370, 681371, 681372, 681373, 681374
    };


    public static readonly Dictionary<int, string> USER_TO_AUTH = new Dictionary<int, string>
    {
{681374,"KQQR@P^({$KKi1?kOh!JfLlaDKKKKKKKKKKKKKKK.iW{)Y^-Slm?}mNNaJam$8<$O/>F+ym-aLR=6Oe4IMEkj)Fg%WJh#c2C?b=0U/DUPV{D?u/{}^})FP6wn?zrFr}LC4+Jz2CWna@HCa<!CgLT$Pd7%:uGBVw$-}Tq6CZ}BH#oX7)Qk8lqr]Ji*@Y/zr&VY@v$qh*>T"},
{681373,"KQQR@PC}OdKKi1?kOh!JfLlaDKKKKKKKKKKKKKKK.op?c{s6eF5*ribaW8}CQle?N-4(?MTNe6D$jzZ]H=2F9hVmvLVab@=qNZ/;^Eq5)$NBO<K>6ij{]Affs2wr4my22%3[#6?MPbzh:0NG@l;a-a]SbSk?s3:c+k]c;a9dj#W1/1CJed%A<0V94ea?U6GT9^Y:$VY4["},
{681372,"KQQR@PCT:qKKi1?kOh!JfLlaDKKKKKKKKKKKKKKK.S6u?{8r-uRn5AD@zVVuC-sas/M]xPHu#ou36-H0xHdCtBe)l}1aFpQh[]/<u#+(kanpiv;Mr!?IeXj[?-s#%H^ls8r}z3*auH@f]kja@[Ayg<Dz1$)ZBT1fGLP1>?Md%Sr6qNsjmBoZ%5+i+r#2!7jJFcE)zt4fg"},
{681371,"KQQR@PC:pUKKi1?kOh!JfLlaDKKKKKKKKKKKKKKK.ke>RHt-BpUCp?l{Ax*(j@:@K0P$KdImKlknL<3]&k?PhY-(h@a&fJQWQ^NtVDE{$feVb8=HfR&8bIu+9NV+M?}q14pDUe@[voOI1cc^et(g}<sTV24r-0a3%+M$4@3:k6:svsbZd&F(jwH&%1/U}rFKUw;R{AZl("},
{681370,"KQQR@PCt1KKKi1?kOh!JfLlaDKKKKKKKKKKKKKKK.Um57T/[D8-]!9lpe(ZO}ES%A=(aWy7WL6zGwGeGpyZV*uktvZ15zhTDOH&g7=9qii-E8F:P9e8%}i1S:^Ybmpsm2=eO#AlA;I5efe!!LrW!te:7J;doi)les;R4KKm25?2?*vyr(pnlTMN19Ll}1dJ#t^yM[&Nt)"},
{681369,"KQQR@PCWGXKKi1?kOh!JfLlaDKKKKKKKKKKKKKKK.lzGViIj@-xvwhY>S%G8{FOy860MKkdCWEUGb6>K-?QdZ/TPGKLN:;C-N-V-9e{OOKP?4ADt59Y]IH^%4bwX8=+DC(J8o?[sBCO>7TAN5#EVP4BRbdR>+C2LCTEI5NWm=dTcc*=2@T#^f:a3)9QSfhdsG#19SUxEh"},
{681368,"KQQR@PCex<KKi1?kOh!JfLlaDKKKKKKKKKKKKKKK.]BSZ^^3cG{{<x8h=d5m@LiN#Tg8PWWu@cf1Y3+XT)USP}}W:-RlRbz<S$x]?bTcXuu;%i:bt&ufAH}c*ip3UFZ>u%alB(rAb0oMWzF;eth!CsWz:-Er?mA6?+FXd+)}ypa4yM-=K)AyGN4Y$vb6Xj[w0aJ)]Fui*"},
{681367,"KQQR@PCQP%KKi1?kOh!JfLlaDKKKKKKKKKKKKKKK.I;;+F[IGQ7wuz%VWFm<XAU]TE&k(jcxrce]h%zQAz[<n8t5u;)[#75mxEbledL^e:59Pk=^pyyvVxPPKypt7)H/<r5Usry;rK#Gi1Ezzyr^S{OJ?]Xdilv^3c?C]>f0W29Jcd:EhnTYSGAl/$pK<?+p%16OgSN-t"},
{681366,"KQQR@PCfB}KKi1?kOh!JfLlaDKKKKKKKKKKKKKKK.yD]5&JTwj7eBAxAlO0^ABUb{d/4s:bEd$b{vm[GJ!r:9EuI8i;Yx8}PtQ;+{^Anb)/$;mYjAQ0q#r33BMszJ6*noSi1GOLU7>(Jc&lG9i56iF7PzTXiDJBC0RP[]p%k;-Ti&0Q?^DG0N+[pO8o1$j6oitKPc;eLj"},
{681365,"KQQR@PCR#+KKi1?kOh!JfLlaDKKKKKKKKKKKKKKK.]5L*AR/[+!N[#S^a)b9CL%]QnOlZ2vw))^E(oZ926D+>XMxAoT-:Qex{[bu<zM(VqyKBdFR5>gRbloH3{[EKgE9h2?^eK=gsz25DZhMWkKT!x/A{sSU(4%OvpJ:+A>LI;ae9*nuNN$}JG-b#xgX:j2aq^+<Nf7n+"},
{681364,"KQQR@PCKvsKKi1?kOh!JfLlaDKKKKKKKKKKKKKKK.IG:YCblWN;6xEF7tP8+7@y&JfC4Al6-GC]yuE/AcW:M9gobg0K3#8DUd{&+J*5ez&aV$d9ZivII}-5<Dr6wAHcE!y0KoID38E22HqcUT;)/?o$+C^C9fb(FvwvFq}pi7IA;-%UyDoFGq*}hH(#X2zm?#N?m&MHK("},
{681363,"KQQR@Pt>auKKi1?kOh!JfLlaDKKKKKKKKKKKKKKK.KyUP5/@o^YC^&e!e>L}:YLIq&f7]3;-?)]cxs9:P3#YEbC6ev+?=NZpgbjbn7Jq7g4iJdZBX}u4dn^&9J9M<uU}lf4ABBf<Yy]#3zOhWSv[mu$l@gRI>Dop;n*d2C9FX:<yX]@?#:3bYpIeE&+!egjG){6!5;TV@"},
{681362,"KQQR@Pt3qAKKi1?kOh!JfLlaDKKKKKKKKKKKKKKK.dp8=7[FtPk(5}2Gu7o0YA*&@?K=]7l^=&*H7sbN8o={c(1q$lbk[2gxan5KE&KdIOl:oVytiMo!ClWK$xV3;M!7p[[d:U1Q&$E$Jvd9ojhyx<Ct+T;FINGrVI5236nus#5h(h2>=jvz8Amy#4*[VJXwFGRKN:H0Y"},
{681361,"KQQR@Ptp}>KKi1?kOh!JfLlaDKKKKKKKKKKKKKKK.FXOl2A/fmGa?X>W/S+hS*UeLjG50?brnX3%WBfupG1hARvGloE8GI(j12;?D5SYaC?3n8a(2IyU9hn>>xiMU=&V5W=BmzNfFNSL%Ow8AH3Iy7:[/}?;hPF?)zIW1@K=PbbEl^BqOOIkl9CV2/j{!7&@PLR2{/res"},
{681360,"KQQR@PtZ{{KKi1?kOh!JfLlaDKKKKKKKKKKKKKKK.E)$&68&v&eEp9DUixddY8*PX:>UV<&={3>:2yng[sPQ1^iZptYefRz}V@L<Y[eM3/96!%vB^fFGB{YKQ9$5:u:(EEt@Mvf@PibHI/Rd%hZ7{mQr59mJxqhQU?CQ;5@RvQsE(]SAq#}AqL!D[*le-E!7cK]bF^}Bl"},
{681359,"KQQR@Pt0OVKKi1?kOh!JfLlaDKKKKKKKKKKKKKKK.KH8*Or%-w@T$sZ0g?XN2he=#Xy&jntLgvL2kSdv%lGQx09d2UjKcV!J{QN$)xI#/Xo$;{p)pkRSr829Bcw=;YG]CM-SNb^Ek&5Y$bS%Wp#[6{wvWp#aT?k^0OoH9+rDWG&D1m>:=>7b?cV>=nY^7L{B=ncWnBJx>"},
{681358,"KQQR@PtM:YKKi1?kOh!JfLlaDKKKKKKKKKKKKKKK.isJoRN9*F?&LIUF-zIKubWz@&5u@V#MB2c-rNBc^l!jN)?:>&#6H)QmDg+09U<[7[wy:1Hc/mPK2r{1E6EWLgKAN{uzbz7(:wah$>rwMBp<;X:Nc;t8(Tk/+j&bj2jD$zSA(A/Np$#Owv$GSIg+wjZR[zAGJ3P7;"},
{681357,"KQQR@Pt!pTKKi1?kOh!JfLlaDKKKKKKKKKKKKKKK.U>y&G!?JvCUA@VzqTTM^-idj#EWeT-YMSfpcZnC$v]oXvez6T^+M*&BqJGEcm:j/c0ksUSI3mMJeY7SOYOg0!ag=p@=VPGGPp#I+T]BVtEGXYUJ*c:8C[Yl?6G:ynT]N#gF/GR}]TWSkJXdQ&kbqb*I/9iEU$+T<"},
{681356,"KQQR@Pto1-KKi1?kOh!JfLlaDKKKKKKKKKKKKKKK.c=$ZGRI3j2N@V4eV7)*B4Vna*i15^{+FeNy*3vKZmbSm]YWdfV2c/S<eQeWaR}*PgEB9WZnNH:&c>#Q?N96;(hYCnW[aBW?onLgjZaXy)(>v2a%)!<NUUTd7c<-!0L1%9b)vxnz4+R5ZqEGOK3bPP7m%y%2Ac6&>"},
{681355,"KQQR@PtdWwKKi1?kOh!JfLlaDKKKKKKKKKKKKKKK.FPo/8!/8reAlDD+]+eJ4HI-^+!lYhqIU)nL#9*<sU+J>6@Ydty>qkZ*(}Fqp}=:(?TYvZiu6>#CYGUhNDJB#!wb{<+nSw332lI@F]9+Dp@Q8gl=D%kecDfU?^YBHl!Ty9t9%t0f9yd#1QxIk16nD(nL>cTOiP:j&"},
{681354,"KQQR@P*s0OKK]pmkOh!JfLlaDKKKKKKKKKKKKKKK.c&c@8L%AY[p>eeuYHxLoF{yV-5i/2$;bs(77m<]OJ]yCFSYWm#+ZM&^Y)qXDzePcs;Ztx@^gzz+qk-KWRXVW/WqPH+^gs[jW;12pHN%i/5zSjU{W6T%gBgL%)}p@+I^^oP0z]21V5=n;6]qPo*7TOVOy2WGyi7^V"},
{681353,"KQQR@P*wLNKKi1?kOh!JfLlaDKKKKKKKKKKKKKKK.d[Y:c!d[8w?XlCDD;9a<u^bw1&o6qZh<rKlDF#h-Q<wS{>c7fG?EjC(uCYNNQrSe+YAKQjO0Spr3WTex3y5Az?nzRPWZUkHa5;uZNT}OHx*WEZ/m)Bw5IY{dlj@>2a{#PT=FTty5m+pr(eppx[UFuw}x:7u)U%5p"},
{681352,"KQQR@P*5Q3KKi1?kOh!JfLlaDKKKKKKKKKKKKKKK.d%VUhf&5yOlDtK)oSjIW:RDUa{o#%:Y;2I{MN=Vv!Q?!2RgJZ{OViYE;=ILBEQwo9H%!^m7xm}32}E%j8CG6Jm*iAcBc[(?li#}*S6GyZP<MsLBv?+R^znBozN12?!$}rRU&Z(2C7R%}![I%Dk&j:R1xC]W2Lyg:"},
{681350,"KQQR@P*Sk6KKi1?kOh!JfLlaDKKKKKKKKKKKKKKK.Gkq(g-Ah-<zKW1=5{%l:=82Pte9?5cM<ZcXxfZX;YUEQ}!xXuufN:z-LtXJT1WSR=q3/4IEM[m*{]]VkVd]2;]J&}5@5ZXmi[[bLfsYlpr#*&j-)y$OR<4:47@?lPDwN^ry4{]q;LKx%RmS@jmPub(/SkZVD@AYq"},
{681349,"KQQR@P*HRbKKi1?kOh!JfLlaDKKKKKKKKKKKKKKK.WuhELgz/gi$P}Ew*I{v=?I$&Adlv/s)>%$4YQlFPxH$[S:-B%nt4?Yqj%RiR@si=4abcD&w68#x]nHAV&xiPXx9ykhKePVd??-pbPV5PEP9#hpI>-^TP-^n-P1fEJEwVEBJ<q<HKJcu2Ozt[gh)=clfViyg3%tIj"},
{681348,"KQQR@P*8d:KKi1?kOh!JfLlaDKKKKKKKKKKKKKKK.)%9MM5rF8b30NW0Ok0tjc399(L[HYA[z)TnQ$T1Db/ZtyLN8N+fL@Cj>1huip+tmDL]uW1vc%UM2v5sEZaC4MW*JI;0FVic9@T{XOfQ%esm[}TO}NgtRMSi2BZ+y/tJz0X-wf;:>X:s%^N>5r#b<3qC1P*qvk73x"},
{681347,"KQQR@P*k%hKKg:fkOh!JfLlaDKKKKKKKKKKKKKKK.lMSG+rfujK^]/s!{!V1}?:#RzvMohK$7C+:/hO(c>MB/n$l)RL@Xo1fuC7OdwQxJhF$Fd!L<!=f)9;9Zmi24&zi]OkR3j<>lX)Kc3nhs3yY2e]4PST?}H*q@g>cA6B!FtMS1PM3v8jh*IwW]Yf9X:?g&A:2T]K6-"},
{681346,"KQQR@P*=>5KKi1?kOh!JfLlaDKKKKKKKKKKKKKKK.B19u4ozVPUe<oI81Sw:QNkhSIRuVoUYrhK^z&M&&=P=l:cyyaD8*j;%xn);fAb/F<6Z9(<sA#s}=&FK=y6E8dS0#C]oK)a8);?7ZqO}cWK&$L[6>>@H(yIiGJ6Ze=b#JSj3fi?s?-Q*z{fXe+(DNh]G<H;P:g:r:"},
{681345,"KQQR@P9%wjKKi1?kOh!JfLlaDKKKKKKKKKKKKKKK.nvHL[J!m:&-2vXMmx:^d(FMNS>=S5a^*@L^WT=p]V[Qa?Ft!H{!I:e}@+4d[l$CywfJxkrCUN+:L6^scceB<mD=CMkIum[UEB8ldxZKjghSE*pkwY^n46#/wRkE]3qM?0>us$R!snCO&xB%wzM^YBWBrVqVjO)b@"},
{681344,"KQQR@P9YbzKKi1?kOh!JfLlaDKKKKKKKKKKKKKKK.xH$2L2$!=)h:TnbklajF-!t^j^}oSIbu5MxbAg9Fk6V:dTEXy6u?[G2=v}zeG]KI1kvY!85*r<dtD3t!0ipNY1/A-]6Vw^#&*;xLuFrh>LKJ/vLhkH/@!?Vs;xh]RAu$oMMi)fCY/qWq^ou9{=qOUEfIYyGeQQn9"},
{681343,"KQQR@P9bDDKKi1?kOh!JfLlaDKKKKKKKKKKKKKKK.qxhc[mmVrB&hXHj#R]oHmgnao?+yQ4$Qge@Z^v1h#^KY%0]aBiVB/&;{KyaHmf=Nwk%Peg>)AXf6-FLlSEqpb3PPG]jyE(IF1c}}nW9?Tiv?9P*bs/n2NsRTbx;JH@SzCIwd#6X$CEhjFQS!I7fTbLLix<9Kk1Ep"},
{681342,"KQQR@P9C9pKKi1?kOh!JfLlaDKKKKKKKKKKKKKKK.]*nh(9yjmb4az4h)P2NgR+k/M(5a*#oy$bkxW<cQfXKQsZZS?QruSh[kiFfcmT<>h}/0$HCK/;Fz&<f;VOUfXzKZ7[DFQQbudkW]CBZuWS1U9jH6<JE8{%cOnKVFAN2=MOa]?LK!8&CvEy:A-[(oeQQZ72*85k*>"},
{681341,"KQQR@P9[[^KKi1?kOh!JfLlaDKKKKKKKKKKKKKKK.I7qk:y(;J?Rl>CP!w<rlYrCVRs/7<Z@{#}PgLusNA*Ai&N!IH4EV)SZ?Y5*[I}x&G;3+p%jl$@*D243w;csFZ!vf=qw-tOjw6kgb3JIG!HTBu{2(LD40R5[idUE79$PArD?TxRhwrnwvOYGaajE?AM{jd&AlO@Bs"},
{681340,"KQQR@P9E2CKKi1?kOh!JfLlaDKKKKKKKKKKKKKKK.QGlq1]X#6baJ<Te)EwVq^?VE&BbamTYtz$cg!D0PN0C6m/G]i*^ReGZ@Segkj1ap5A3(gj@57]%x=;}[TiWDec5Uss(/JQE1)YhIRqfuH5%6aw(P$KH44az0]rtKs]1bG)dTK7%;8%GOl@iQ8ONaPJSs5?b7M6S3"},
{681339,"KQQR@P9gHtKKi1?kOh!JfLlaDKKKKKKKKKKKKKKK.)9yh$bjl1N;}!uWa)Lcp4fOt8T{;x^4u+#^u;-PUi-1*=Lw/&XDj{]p7uO71@+6c})gi:vLSnoqxOo7@BWhC[TEWn[bG#ZAEPJfpSpn0*e2)Q;KZqyO74<=aj2>]zZZe3)C08a<OkgX!0A7TW5C4CzwIxvoGKU0+"},
{681338,"KQQR@P9]g*KKi1?kOh!JfLlaDKKKKKKKKKKKKKKK.RWxNu0V(%h^FcE+Jbsb]g[63K+)^2lvM?hg/pZ!6uowz-DuKOdtdCBl1Gb^rJFUIkP(:$b8;/$?ISr8!0i5RE#R[x0+87k)zL&u3I!fIJkgikY@uSVS!XG&e2WI<Bo+yLy]/UIk#rvpBkdbY]P=G{l4cp:@P*&*z"},
{681337,"KQQR@P9i/9KKi1?kOh!JfLlaDKKKKKKKKKKKKKKK.kZ=LOK#Z/jCwgFW){m(A){:^7pALphZ4WW<-#O@OmD]kdUWUp^0}2m2mMv/@[dRfYHtgzYP@=]YD]6y1gucHI4UL(iuR5PRBjb2{c-(o25>P}gFwJoGbZqJp0};!VkcWCb9*qRNDKxrOzm7}@jt7FExaKUuPNbME"},
{681336,"KQQR@P9U?1KKi1?kOh!JfLlaDKKKKKKKKKKKKKKK.ITaRePR];ct@4?Z%f{&B3h?GL7YMyi;@t!VpG=!F[{@ri1Y#fR4%-N)r$ptdx1D6=(UWo6{{Vxu*WX(jTG-:q:Nb9q{]ao7h+8kJEaf4LA84/i}}#*3V{b:e}aA0k6f0}HMK-acUK6kxL205DHj(a9&BFEfgK7al"},
{681335,"KQQR@P1AiJKKi1?kOh!JfLlaDKKKKKKKKKKKKKKK.[fBp4^hMY]9sE(!7oVq+fUM2M&Yc>^JsHqiCd3DCmSWcB07w1HF06tEgOl-6zQ=f8v>cT4M$%nIY}bIpoHrfjQpUSlSP(Cv*xUOHukO8PLBcM?&L1S3IgkWkBiNakqp1[;+NO}xm<)R-Fmu-+SC1rP7cUpwiLNjZ"},
{681334,"KQQR@P1N$4KKi1?kOh!JfLlaDKKKKKKKKKKKKKKK.2%uh!)HbnmLBGy:#3C))fL9!Yf*MdlNte](^eZYpPJ5hI;ZJH!ZMk9v8+Kh/v6V9NGC^-}FE{vH)IZeYS7J)Nff*IJ>a*7V]&8T1&:?KM*0=ilxlbtA-r-wOrv6vmezpTHIaX4m:4avjt7UainS7)#d@uHV$6:<b"},
{681333,"KQQR@P1D<ZKKi1?kOh!JfLlaDKKKKKKKKKKKKKKK.&{)ny:<^VKd=(Pc;LO{z3/F7F%(FLiDc]Qpt(4k^6m9gdmXTm>}-mpK6(TX}W}YPhmJ/G)(J9(6&UX!@x?AtwkWw#cPLPo^l/m7aDT+5VtVD=)PZ$!}m#%:QBv&!bKWZdWVS>/PA*W039/*>gf!mVa>07v]-i9?y"},
{681332,"KQQR@P14AnKKi1?kOh!JfLlaDKKKKKKKKKKKKKKK.v9gJ}?i9<0{+t23W(Wx9tT6b1#(mG:T(u8;hB0L=w7ZY>FYlP?>:mW%r*${#oRS3fwK&#-f7kzU}1X}1lXPdMx}wm(tp6(<EqZYyR?^QO1n?Se?xj$f9Z$xsq<TYiv(9*=7mS1KK]>I<i*:z^+szHYC?v%@mjsg;"},
{681331,"KQQR@P12-[KKi1?kOh!JfLlaDKKKKKKKKKKKKKKK.8(u?mS#rx3iQr#RTP4#5]?xjqlVL+ncWl]JBc:RSat^$w14st217)y?ipUX;+qzTv#FfD+q+#Y(^J]<[MotO[pCV)&B^eo6vQIA*V{37>s)ILzsxddVPl1XGhzDxqHXw#w5pG2aD<<}*}PGJv4bR!Hm::5[j(0lu"},
{681330,"KQQR@P1c6WKKi1?kOh!JfLlaDKKKKKKKKKKKKKKK.o3Kw>3jj8S#-ZZBlC-CxGQ<*r3Tv7d:nV9Lk/e+q@#l&8jMbCPAx#sus{XW:THWrq[&1f%]?3(V)][vs}b5<F2@8eYZi1<@@ephX[<Goc[Qzg#dUM0wMp4XBkbr[U[uNX$P)yvmno1LkU2m<rC+fo+J3qypwY5;x"},
{681329,"KQQR@P1/zGKKi1?kOh!JfLlaDKKKKKKKKKKKKKKK.c^%yYSla!g3ZRJU{ds1rlqVsilv$nj9ZwKZ>bXW$zrG76J8N:58WYa4#mG-UUS6s#T{c3[a{K7!Y:*dj8{N$pS[DlOp;weDkdQGpETWP/+gE2[/S;{XCEEA]:dlp3zU;xiChQKNTeu[4w8ovQG4QIptcvVR>@>Il"},
{681328,"KQQR@P1I*SKKi1?kOh!JfLlaDKKKKKKKKKKKKKKK.ynynB@C2nx-YE2d-]*hXY:[uKc{vog!rNw*{12Mhid&MZ#;<6GmmPm;>OtbvUKl+W8Rq;ByEo!/4sCS60Ts-iJUCkatxlKNr2}Mv>PO!ZzeaT:fu6}=p7}Qg7>1=4d8F1bGjpP(g}iiue@Yd3n?i}pxn?(+L+wnj"},
{681327,"KQQR@P1$n7KKi1?kOh!JfLlaDKKKKKKKKKKKKKKK.e;%xPdQC+m*/pHik<E$zPN^?*]B!/i*a&HaxH)@1K%Pat]iRPcKqBo0bZ+-:q{I5DAhIa:I&;xPDz2*4?GbY&E)68UOj8#Dn19[k=w>iWIuotjTv//:A>%E9SqB?m@so0;oGA&EmsHu>c?0XN&X/<]35p/yeG4eN"},
{681326,"KQQR@PJ+yyKKi1?kOh!JfLlaDKKKKKKKKKKKKKKK.8TEce!$D3X9fGQIGiS>R?O^vZ-zz4B09muzgFUjMRDpaSDmdImckL8r*dqMOWxCAu}UGF)3h+e0&P%-;nkV=S]D1O@};m9OGf3;5f@+c5VQV&2T+lVGb:oA4b-c6:+MU2Gn^9MIOI0f@sR;IHT&uAW0oz]l$);dn"},
{681325,"KQQR@PJ-;2KKi1?kOh!JfLlaDKKKKKKKKKKKKKKK.8nhS>5<N6y$A&l0cE0QC:]bcS5NK!y;N;pvElQD}^W2s9S(Q@(IW)N!DgQlf[Y%}vJd/qoCRzLbPt!nTGRt3j?g>mJigN^XZ/Y:^8Hw*i0V}z)A$C-Z(p(frumbS:6:181zF/xW]5DcEO9^^QWf(7v:Crj58q0&M"},
{681324,"KQQR@PJhF0KKi1?kOh!JfLlaDKKKKKKKKKKKKKKK.ybBrFC}wQr?7q1Btb/J:W%;lW?1}wU$Wl&8yMAU&d+OB#g!Oav?O{aAk$[y#*$m)eg=n--xsp?C8M[6]g8j/-lz9b+:R3$VR0=9%}g:wcD7%G@4Vm}-r00-zYL%VJf&d8>mDN!zW})=30]V4qUQdfcdbGDUEW*?R"},
{681323,"KQQR@PJ*)xKKi1?kOh!JfLlaDKKKKKKKKKKKKKKK.R:VRE9:@VMEGk$&3@@&aq$hgA##(i1h;rRsObl=H9@hNY^ebZ{IT{5zf{R)+VE!QAryEY9oB[-7GDh{beW9L*T4*(E(w<HLxgK?j8G]aD1fi5@V:wJC$k78^OERhECG=l;V[FGblvZy7T@6*iLeKqIm+xO6OR33]"},
{681322,"KQQR@PJGmEKKi1?kOh!JfLlaDKKKKKKKKKKKKKKK.WLzlJp5uWcR/@}IDTIh$2y}^M{aF[(+chUXB2[0AQD:!hC]*^ZDc$Iq8soUbDfV8E8xy1RAc!#%nhx&f#T428xeI%3Quh%G=w+]=[cCGQ$}})tK%^g5?l%f/APnPAKa%??*S36jUxJOe(M0q2oJabsv2)pzn>v$>"},
{681321,"KQQR@PJ;@eKKi1?kOh!JfLlaDKKKKKKKKKKKKKKK.Q!j6][0JN5Z#9p{ptXrg=)$Cs-ILF0kl{2y]BRgyy#?^pgUD*jj!hhP![*tdS&nW2g/Zkxc7]U*j*n!001xD8cPz3wHuaF6miz{y0-Ob3DVVGPAH@BdD2hwFr;xZ{D{r}]Ix70D?!B#I6Ly2OBesrN+ro<d6h&[/"},
{681320,"KQQR@PJB(;KKi1?kOh!JfLlaDKKKKKKKKKKKKKKK.2v>p:9<eKfyU0eG$qrHIN;%2=P7H%zG4ja$Py*yqZ/H>BO/W(&LO0kgDou49lDfCO1Oc<!(FZC5hwNkk*A(X*7&QpTCUiwA>0;#@b4z0/}Q@**@8XV&A>H]K#1m-ol5cu)WX@#Qz<*M$r2Yv24XC0Q6;rbRnW^nF"},
{681319,"KQQR@PJ?XHKKi1?kOh!JfLlaDKKKKKKKKKKKKKKK.I-FFnKIx;{l4n-&i@o-9Qt>?YUsCn@g/sS;I7fmLSt%Z>C]#<%6[iQXq^w}yO-H+y5j}kgvFZkp#[L<xnB?;)}pcuut$-VLb]RQUpGJ-47lXU6Nv-^1nnxDP+*Aeh3@t)=qgU*NyG6?!c/+SDDA2%6tIoayVQiZT"},
{681318,"KQQR@PJ&uLKKi1?kOh!JfLlaDKKKKKKKKKKKKKKK.WjSzk(j@uZYlU=:+OE2DwiL)m5sFHoJ<O>J3UF<@7cHqEzl}jE9gT<Dy!;VinmvT]2Pc/mIVyGE^CAgqx}Gu-xr0:Ll/)>GO20KbuVksYW3iD>=ldgOP%IDpQIr&p#:?k^?6=f}HT-JBfY-[ao>@Gs?o1iY]nfse"},
{681317,"KQQR@P4<TPKKi1?kOh!JfLlaDKKKKKKKKKKKKKKK.HFVbXBc8]MA#d%a]emdDK%qX[r}CLRY^-QZIV3#yO:iM;=Mo/T:6FA#0#hQ&%U]lJa]L@$2O<fAGsEP(5RwclC/3/zyV^Nwe);0(FSz>lC8K7$WqY@n}8ft9JA<qU=WEllWQZ6LupPDuU2OG[HRJwSMQi#g@RoG+"},
{681316,"KQQR@P4VrcKKi1?kOh!JfLlaDKKKKKKKKKKKKKKK.!($JD/%B:{ka#Bh?:IyfM0LgYcCUWoF9J7WEMIYGAsLklFCc03#+^$C02ttd/uwfu&;5l;?cy$*;x@Jh%#41?sXeayUFfYOr6NAs0^4@S![p%y5W*qQE?+&7?&8h28D&uR;U0nZ&D@qYg8B842n0icG0KcOnYoF}"},
{681315,"KQQR@P46jMKKi1?kOh!JfLlaDKKKKKKKKKKKKKKK.Pre+:j^uI<+}jgsCp[s*2xv^fG@Oe$B%7GG*!@A0fjSV5)2E-w4R67]02c>V(f?n;9DxaGWA/wg&#8=^5Pd^VIEIe9k^-DDh/if;a:::6]K!/)xaDsJrrLU:hJU[h2GBHYT(xWnjAz8%Y%+eRKY3rez8EC>a6pDt"},
{681314,"KQQR@P4^tFKKi1?kOh!JfLlaDKKKKKKKKKKKKKKK.)7Gf{R>!ALlsQ>yBOIeE#v:%x-{/)W[q!k]DM>Q!v&K6^c8CS%=o+NG:#Rq[ETkAB]&trprXaH47TX}A#9BZfo?P%6fbZyFaeMSyE7ARU6l!#<Sa^u&:JolB/nb!ba*yg3YYy!1HeSBi0e2{-VwzMto?V9zKjr6V"},
{681313,"KQQR@P4nZgKKi1?kOh!JfLlaDKKKKKKKKKKKKKKK.?>a;e=aoeR-b#O7SD5l58r&A5]nKSj+<vbCV>={//WO?oc#>y0@R(:y!$1#t=qW;F}6X2qiYD4^L{R3D-^<GcjZUe$[6}9Pl+[GCPklS01!]{EENcJFv(S?@d5c1T*[JU?/(5XN!yCTNZQVlpWNAp<3?4$HlgW8d"},
{681312,"KQQR@P4x7QKKi1?kOh!JfLlaDKKKKKKKKKKKKKKK.0EOarB$0Yw@x7F7!OEw$E3Av+fB^vJepnS=@RUb%$$ewPo<BI3ROgSc#nee&ZAPhF:C[!:pm}Lj*]88u[(vbS>rf/zaU79R-QuanIV&g<Si]jKGtcw45+WP^T2EMi-AJmS>{$b^LoFxC5?BuS@/+2ak}AOVk/WZh"},
{681311,"KQQR@P4FeBKKi1?kOh!JfLlaDKKKKKKKKKKKKKKK./FaD{W?9pRVpgr&ObTJ%WN}>S9OYe4G)qzP7?u95Iu!9N0*YCH^#]0XG%}(!:SC;dYfhm6FM!a#:LHetcFMj3y13t$]l-R#mkwmz#!SvF#sn[JONmVRgEA/vGp?fx]+6htTB#-[Z2OE1dovf+-gJ)zs8Z#hxi$/M"},
{681310,"KQQR@P4#M8Kq>E7kOh!JfLlaDKKKKKKKKKKKKKKK.!y4lOtTA=q=a9S/Qz7k]n$J2Yy$IsD%(+bTeR+SCSfq^doI?7g^p!TD8ArzbZMgSY3mbpTtflmJ(Q<g=t&GQ>LPOk/!I<&/B[N-n;upFOuD^30jX4Q;*wLKRg!3V:fCc1Q*dPP{0@h#dIzK5pYc1UI!zNxy@TCEu"},
{681309,"KQQR@P4@llKmG@RkOh!JfLlaDKKKKKKKKKKKKKKK.aXShi9G>NEc/VKX1-=IQZc}UFC%EALe5O[=t0:h%El)7B0]pOSxwijEQ;<KQP4Imku7(iA5zc72{s%x/78+Suoa[!q0DLnX;XDYX[I5tL&TB$MLx(:*[2XrKQ[z;%gC{M?B6zAeMsp#j:o*zn4vPmda8m=OZ/Ys%"},
{681308,"KQQR@P4qf)K*zRakOh!JfLlaDKKKKKKKKKKKKKKK.)zH:mW]1-JFATfh^#k#yYF[FW2>q!k=*Vh@PX3alW9fvGqI^tB3Ye<BH1{y@l*O6TrnPYH0IbLps>WIAJQ=OD/wh&TuK;I0+*VSXcz&GK9{2OUA&pJI;UNGS^{CJh(^4scd8-HrfuO:YgtDeMls^*9$L=v$d4Qxl"},
{681303,"KQQR@PZyYfK4}*DkOh!JfLlaDKKKKKKKKKKKKKKK.o]by0f})bn:VQ:<40so#oZ{H9ajXD@p%cge/Nn7FIuMzLL8FHePi&-aZS!wWg=?(n8Uy-1ps/tqqfuy3oQSz2Q=7$Y]8jm=99y3oCmHiTBzaNXqe(&9{vYG[1]TkV9&N}6&J}eW54;iXcdPJr-Il*g};L>6&kMX9"},
{681302,"KQQR@PZP3m(e@oUkOh!JfLlaDKKKKKKKKKKKKKKK.!Hah9&vi9qTuwh/?GlO?Ckl!VakLI>6vJqM>6d)VTw{q-[Mu%vvL!bQE4#8j^s:osE&bnBrNYC%gz#*?e#RCP9S?4:1^-}7Wu$t=kZk48P/eFji$yp)-gPIXYP[(sEIB7LEF[mNcArW{8=#JY2U@:;!tDyve:7!F"},
{681301,"KQQR@PZ)5?KKi1?kOh!JfLlaDKKKKKKKKKKKKKKK.e@u}StM-]A^9Sr%-No+oGyqO5)xS/<fTK0E;IiU7OVfhvL06gMOq^{]()SvdnRSmv4<8p2DgOBSHWpeqfcQI7jIYjYRGTfd59[Uyr-c4<0F:+JK@]8&s$/?;nYc*WT3]mnjsQs64p#ZE!Xy30WQCSBp18G^zt<[z"},
{681300,"KQQR@PZvCkKoLh[kOh!JfLlaDKKKKKKKKKKKKKKK.ctUi-+P@pVT)aCYv@DOKAhu!<(/Q@*tDDoJF:01=*5TX+0IP!/MP({/;q!A$J]t1e*(kUrEe#!HD1$1ZYS7}m!t/ERuA#Iod-2d)KitZ<mcxwAUG<?B6t)m1Cq4=w1A@mtPPWW-(NHDZ)R&#yjk<M8G;%%}xF7Jj"},
{681299,"KQQR@PZ(4vKKEO]kOh!JfLlaDKKKKKKKKKKKKKKK.v>Xil?;*llO=p*Ol}Z!ske[:?1>BSJ)9-)mDty+y=pBnGAQL#vv-Af5dwHls>f[?sPd)SFaVTC)BhnOhBA&^!gVfV7{EWJlD&mp/h:VyuycA$qe2zeTP-rExmUbOTJw8fGbx/=y$2{mtT:yr3&-G5m53tN-s#$+o"},
{681298,"KQQR@Pn}SIKKg:fkOh!JfLlaDKKKKKKKKKKKKKKK.F0#-qALYaS#6gXz+!T$NjjheAe*^Im1@vrVpgw7r56SezufYp:Bwtdk-Y&FZPjA;m#8h?(snw4ybGd0+G7hVdq8V61[3p8:Ol#[Zy4#-8Ty:]#?4T8#5%?jHba7C<P5l:^^/&uDo=tPN=qlZKZ5cRa:r=VLNH1P]"},
{681297,"KQQR@PnTEoKK]pmkOh!JfLlaDKKKKKKKKKKKKKKK.(](s3Rr$pXWK^octwtm;MT4twG65Ozb*#&LY1b&#gCsbHoowl2Ol>^uFl$aCQNqVYM?*uXg7bI+;mH2/4N0{w+ISyatX#;7f{7dXuDFIbwD{OZV])A2v[eEXDv!mzV}flJPLas+j2G6%QZ8f4LlB7:bFZr=8{S2e"},
{681295,"KQQR@Pnt8iKKg:fkOh!JfLlaDKKKKKKKKKKKKKKK.SP8z$3}u5*r6HpMf^{#:[dn/(kz#9lI;OWc6bsQ[}yBZ=7R*u7E/A=S!](GkjWdAVYUY6;cV#}Zbxyj:$*MI(4mk<a/GxI:I8WMU:3MnbMA=tLE%a^hU4KQcgDQ<ynTl@FD$v0@*;Qs!5O}EJvKjL(2yeZ^(Ma1Q"},
{681293,"KQQR@PneIaKKi1?kOh!JfLlaDKKKKKKKKKKKKKKK.m4N:S>N#j9p?Q0Nkbtw<7jEAwtTFD2MibIwm%ABeRMj9vtfNzU>qV2/vaPm{a@i:SaRqli;utKkl=M;=R?ePs]}9>]98C1PY=]uuuXXR[lRn&WSfeLCIQa0C[P(mZd?<ri((Ng}f*74;oOXIxv*Gt}=5f9037^bP"},
{681292,"KQQR@PnQ&&KKVa)kOh!JfLlaDKKKKKKKKKKKKKKK.ESf2CjDs@DvVTb5W0fE/H7ozxT4j6B0gU@:YqKRdMmP{%s=s(QfFZL9FNf18:MD}gKiGpgs6TJW6gkrPN^8IXi{S/=jN73g5Qt03@H!RiR&z#7KBcoAbpfllpdISBlK*Ox;$bmX@s84^eJYy0$lg$-Q%ATp}klCN"},
{681291,"KQQR@PnfU=KKi1?kOh!JfLlaDKKKKKKKKKKKKKKK.@!F(suFp+z7lr9OUW9u/pssS9RCwUR9O21]u:#(*+VqcLM*;jo@OGbEa9JU[y;rn7f7Gc?Puq7-CxR*!zD;VbL&:Gi2aH}Tv9GCi?i[fI&ZM1KGcUXqx&YK=lmAIZ&N{T!1xE=;C?&gtpsT9KvHqI3Rh2#%i5tV2"},
{681290,"KQQR@Pna+(KKi1?kOh!JfLlaDKKKKKKKKKKKKKKK.y!r2#:crmq[Qg/cKH94qR(OpitsR%E+3p%:ahssDc9-)lTyfQO2g9QjPPQSo8i>rR(nGQ^+3dku7u=i$GZhS]:(pN16V^wLqe5g1J*+i{@Wy5$s?dnpRPe(3a%AybE?dGKH*mad]ilvt5qsMTseK43iFNC-^AID>"},
{681289,"KQQR@P[XV$KKi1?kOh!JfLlaDKKKKKKKKKKKKKKK.G]uDT!fH>hitAtM)7r?&;hrG:4djXw6wF9<I$Iwhz@J8g(;k3$@X}IKVS@FeJ?B+L=q{QIm]*dSmNK2ye:s{GZhU}cGx&#(Z1}n&5?s39&OQt<9f07tuw+PH)Tu@o)ZYR%z62Q[yJPJ>R%o]L(sX/;SGmX%u;NnP"},
{681288,"KQQR@P[{NdK[]I)kOh!JfLlaDKKKKKKKKKKKKKKK.$a/++QXA>-wFQSFo(+F^9aVv)u3WtgRdhrEd1g(h(&97KnT&sP2ub@;}hwb*q9D8Dxbd<7pTrb@t&0b?$}>UyPAOwW9cq7eibl7AE:)b+R/rzZWko1;1DAP>c#$)u7Gb9?IQn<)1dlc5yezZ7gf98y0QXg-%kosJ"},
{681287,"KQQR@P[rhqKc=R1kOh!JfLlaDKKKKKKKKKKKKKKK.7:^yr3l@<#Ivf8DHJX/aVB>9p5A@Z71y1o[tvEW@YbkTm/)-j<Y6$6mc>CM2@4!!:$9:*q#ufreCZu[}YJUGU@E4$*Qz}nC^UfUs5SIwLkaa^Qp5to6D)DZeR/dgbp^A&Us6S4tUZvC!2P4d<NKi=O1B:VxG%Lk}"},
{681286,"KQQR@P[p^UK6Cq;kOh!JfLlaDKKKKKKKKKKKKKKK.qXU:4r;;t/D(=jPKd3s}Gl6UIj0Vy<h--knIjLp2LsLdukoBTZ&PF@u}+oaKG<yFGR0;JD;5$5sOu4C*)0A6$g)-<BI&b4FP*(nwdZNv0l8vJ)Wt$<)c@WK53*}3ut-SLH1v^O(j}[7-&Lnv<XcEO!oC5XO(J"},
{681285,"KQQR@P[ZJKKK]pmkOh!JfLlaDKKKKKKKKKKKKKKK.MG-H$tGEr>hBNQ^crxlSu+[sUWe(^):fD&/iYDBX8&-pR&A}J7N^q/PS9:26Z<hnlz7<jw?4L:g{1--$)lbZ@WbIj0ZPz{v/-PE5mb(&Jf&CbvKHts)s!^OEDJJF<xJ%WJ1FY9%uw1Bod:oAzjcktif&uHXponkk"},
{681284,"KQQR@P[0SXU5Q7BkOh!JfLlaDKKKKKKKKKKKKKKK.lIVE=b6N(7B=w$YRoGacfOA3nD5JUd1>^5Q{}/H7!xmG(:SRMGx/P&:^q3OSapffqPB?Px0wPT^NQO)I]!cZv@<ni^ZFNb!chwe+z>S3bmz&1N/@<r!/hJK5*c2*}cIZC3N0i2L{DSggJ3@Ro-#;4K-WTR*W]Z;Y"},
{681283,"KQQR@P[ME<K$lMCkOh!JfLlaDKKKKKKKKKKKKKKK.R?0MTi}j8c*ufEVnvxTKYu9}G8&fh=Y+3Mkw9<}g&q!oK7i@%{y%nr%s1W0[t!Oha?^^6lL8qQHDPwUB/jI+4*hwr2[aH{3@n[WD*@)3[B(V}SHaeX6R>?T0mf:0B{B}wqpI^)CYAz#}IBZ4n*0MkJ7!:Gc#7qmW"},
{681281,"KQQR@P[o8}Kl^Y>kOh!JfLlaDKKKKKKKKKKKKKKK.$^;;6?&LhHg=*LQe<<SoU}{uZ5K]gGCe+9<A[I?wfps#H6z[87?Bw>])apTO2gmV?8hOHK$e@5P;UH5YVoNj9eT3@XzCD%J6ylA^?xHn!I=j5Gy(c9UL(j]v9JV0(>DsqJ-Qc?/w@n+Qp&z$vrT$1UTo@T35m=c8"},
{681280,"KQQR@P[d]+KKVa)kOh!JfLlaDKKKKKKKKKKKKKKK.llE^*G5(yZ)4Esk8+EbmD;BUcDmlVT+mh0O:xbwf1HR>^DnQ(yYBL)2-$/v46lO16]T:@Aj?@3Q*(#R4GD$bH*T0&WZM4IeUGnQZ64[<3/ErO;D%SXfYmbndJ}TD#gAsfjsgQ{:uL&qnS{QK6C7r?yF38]v0ha&p"},
{681279,"KQQR@PWsIsKK]pmkOh!JfLlaDKKKKKKKKKKKKKKK.]KR>NP?LIR4V]7Vd+HBa{J+vSB-/mh:s&qgteX@r8d4JDKHY93JA5wdj{xEl[1ov[:x7lIliKq]n#:k;=XUQXs$0/SPLS(}piP2@qu;gj!VficJlF1+[5j+Ljaa3^+uxYf1gTrDUB#I+GV2QbkQCH$G{1lh{3n6#"},
{681278,"KQQR@PWw&uKKi1?kOh!JfLlaDKKKKKKKKKKKKKKK.IL08VnTb*hAdC<#HP8n%VNC=Kym!+F=;^:rU49cV97U6xkV(Zs4lhE;+6scBOJ#S}SeA}LvQ#[]zeWkP58D*7QS(Tbg7BZw@W:gr7e%h;SmyO)VvUXCb#&-U{]s4a06(@]#S^]K}9Hji6rl0obp}*;+;A1tG@{[&"},
{681277,"KQQR@PW5UAKKi1?kOh!JfLlaDKKKKKKKKKKKKKKK.E;Ca+VG5x?L=b%W(6}{+!<y0[LXRJu(OuD!GpA/>$$]}V(}08S[Iqgc9[GiN53wPU/YwzmL!*u)K>CtUhd1fqshq?6slSNG*=OV9Q=SjB+M)W!PvC4#46Tx]0n!]n({fhF?fwZT}!q7tBDOQ0mnrEN95]:Up;h!c"},
{681276,"KQQR@PW1+>KfDA#kOh!JfLlaDKKKKKKKKKKKKKKK.BB[X%B-DL/MtUkb7+[?gZLTDj9[:Zf9Du1m^I6BImRmF==%L(Q$tzOgcj*SIJF&/$*IyqMj#NwML]g:R+TODUMBBrZQK*a!S5<H$Mk2a2mWNF^fDMMxNI%0^xq4(irZ*Ms*=RfOH^UL:?KVKgM1Q#0}*$aLTr4d-"},
{681274,"KQQR@PWLNVKK]pmkOh!JfLlaDKKKKKKKKKKKKKKK.RqW2=Je$%p>S*L{wgdH$]mZ66Ao3/X)jVQC#q^bx*@iF}D(CcE&@p*ognR%RV-XbOapxt=ApGCMv$CC7nMWXpagbQ$EaRUplq>ZEokSK/n=@*<v8A#AWQE7J4W8a#U-PTEKX>8{fLK]qc8:(Ii9^XuR+vu+kgm[F"},
{681273,"KQQR@PWlhYK&JnUkOh!JfLlaDKKKKKKKKKKKKKKK.FKrAvHzK;a1?rl}%&a9Kz>gMICvw+m^6U2Yt(U3lUgo+UMP)}=Vh3O+0d}Yc:W{W)RD1vi&{30OMc]b^:DCDpdFqTEp)%%o-XA/+h^#}0n?Xn+7-9h:8W!]tgq+7hy2$7d9I::;;CfTC$J+V^)R!(wT$Nr?Gz5zr"},
{681272,"KQQR@PWk^TKKg:fkOh!JfLlaDKKKKKKKKKKKKKKK.cOu<IKzG#c=d>ITD{s+q9Dtu%Z[]5{}8Plr#ripQZi5yCR>OmvK]XScJC{B{>=Z(!CzrZGh)*!%%&}0sKaoLlY@]2@k:2n1${I5vUb??1btZmn7{+>V;gb&B9*og4vJc{5pptDY0:m+6g+2j:alH7XqCfJ^Ts*IU"},
{681271,"KQQR@PW=J-KQCT]kOh!JfLlaDKKKKKKKKKKKKKKK.ce2>VFuCc>u<0;@et?sR{n/[2FCwgqS>Al8+<LgTh=xqjpgKQtDm0GC57lJASZ3#P+CU3;JP!g*7o7a6%I%TI}OPNd5chrZ)TNSYH*L<yi>Z:sds/UsGgl!=GK;@$At:+sc/i)#s?m{=)hOc!pRE84+gtI%@F4/J"},
{681270,"KQQR@PG%GwKKg:fkOh!JfLlaDKKKKKKKKKKKKKKK.q:6#!/uVfQmQ</U/Lu-D>Cc@!PYE>GA<;c)*^igpx8E)e[/%q%F=)MY^}Q:UJ(i]D+s6r8VvgO-9kUkr@aD=9/HFU@aQgfHzwl:6L3H%BGh^2VlER3xlIrev3E7@Ut}BKufAHGC?v>BE[+Zh>}((fjWxvH{kp3JB"},
{681269,"KQQR@PGYxOKG+B@kOh!JfLlaDKKKKKKKKKKKKKKK.Uvn=}Ny$u:S%lJNfNfjWwEJMsB>cWVS+*LWO3$b1/Cbj8hMo+bqp0d47G0l!J{pek]PB>8Iofng90}G*86j*yKeR+fs0wrv7%xQXAwj#AbQe}Of(hf^agA6}EPj(oU6kTq?c*wHM)56}UCkbut&7*Ebh(d0Bji)^"},
{681268,"KQQR@PGbPNKW2^?kOh!JfLlaDKKKKKKKKKKKKKKK.W8SVZPrxr@dZ{8*hy]{2^bkrqUQsApUea/yPZ%00vN*2;4UYXd0db+OrW9cyruUx0)SkaHidZm*oMLI:C1sQfdzFj8{59F2z?Z@/RjymAMjtpLx;w8wvfm0eVZ740dEhO+-r}>AoHxf*8v51Mz^u(OHzXQ@1&J39"}
    };











}
