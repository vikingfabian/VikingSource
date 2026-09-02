using System;
using System.Collections.Generic;
using System.Text;

namespace VikingEngine.DSSWars.Presentation
{
    partial class Thai
    {
        //QoL Update
        public override string GameManual => TextLib.ThaiConv("คู่มือ|เกม");

        public override string GameManualTitle_Work => TextLib.ThaiConv("งาน");

        public override string[] manual_work => [
            TextLib.ThaiConv("<h1><img=WarsHammer> งาน"),
            TextLib.ThaiConv("<br>การ|เก็บ|รวบรวม|ทรัพยากร|และ|การ|ผลิต|ทั้งหมด|ใน|เมือง|ของคุณ|เป็น|แบบ|อัตโนมัติ"),
            TextLib.ThaiConv("<br>เมือง|จะ|สร้าง|คิว|ของ|งาน|ทั้งหมด|ที่มี|และ|จัด|เรียง|ตาม|ลำดับ|ความ|สำคัญ"),
            TextLib.ThaiConv("<br>ทันที|ที่มี|คนงาน|ว่าง|พวกเขา|จะ|เลือก|งาน|ที่|อยู่|ด้าน|บน|สุด|และ|ดำเนินการ|ทันที"),

            TextLib.ThaiConv("<h1>งาน|ไม่|เริ่มต้น"),
            TextLib.ThaiConv("<*><img=WarsBluePrint> สิ่ง|ก่อสร้าง|และ|การ|คราฟต์|ต้อง|ใช้|ทรัพยากร|ที่มี|อยู่"),
            TextLib.ThaiConv("<*><img=WarsUnitLevelProfessional> คนงาน|ต้อง|มี|ระดับ|ทักษะ|ที่|ถูกต้อง (หรือ|สูง|กว่า)"),
            TextLib.ThaiConv("<*><img=WarsStockpileStop> การ|เก็บ|รวบรวม|ทรัพยากร|จะ|ถูก|บล็อก|หาก|คลัง|สินค้า|เต็ม"),
            TextLib.ThaiConv("<*>งาน|อาจ|มี|ความ|สำคัญ|ต่ำ|หรือ|เป็น|ศูนย์")
        ];


        public override string GameManualTitle_Soldiers => TextLib.ThaiConv("ทหาร");

        public override string[] manual_soldiers => [
            TextLib.ThaiConv("<h1>ผลิต|ทหาร"),
            TextLib.ThaiConv("<*><img=WarsBuild_Barracks> วาง|สิ่ง|ก่อสร้าง: <name=barracks>"),
            TextLib.ThaiConv("<*><img=WarsWorker> คนงาน|ว่าง|ที่|สามารถ|เกณฑ์|ได้"),
            TextLib.ThaiConv("<*><img=WarsResource_Sword> อาวุธ|สำหรับ|ทหาร|แต่ละ|นาย"),
            TextLib.ThaiConv("<*><img=WarsHudIconProgress> เริ่มต้น: <name=queue>")
        ];

        public override string[] manual_food => [
            TextLib.ThaiConv("<h1><img=WarsResource_Food> อาหาร"),
            TextLib.ThaiConv("<*>ทหาร|และ|คนงาน|ทุกคน|บริโภค|อาหาร"),
            TextLib.ThaiConv("<*>กองทัพ|ขนาด|ใหญ่|อาจ|ทำให้|เมือง|ใน|เขต|นั้น|อดอยาก|ได้"),
            TextLib.ThaiConv("<*><img=WarsBuild_TreeApple> การ|สร้าง|สวน|ผลไม้|เพิ่ม|ไม่ได้|เพิ่ม|อาหาร|โดย|อัตโนมัติ คุณ|ต้อง|มี|คนงาน|ว่าง|เพื่อ|เก็บเกี่ยว|และ|แปรรูป"),
            TextLib.ThaiConv("<*><img=WarsResource_Water> การ|ผลิต|อาหาร|ต้อง|ใช้น้ำ"),
            TextLib.ThaiConv("<*>หาก|คุณ|มี|ปัญหา|เรื่อง|ความ|อดอยาก|แสดง|ว่า|คุณ|กำลัง|ผลักดัน|ขีด|จำกัด|ของ|น้ำ|มาก|เกิน|ไป|ให้|ลด|ขนาด|ลง"),
            TextLib.ThaiConv("<*><img=WarsBuild_Postal> ตรวจสอบ|ให้|แน่ใจ|ว่า|เมือง|ของคุณ|สนับสนุน|ซึ่งกัน|และ|กัน|ด้วย|การ|ส่ง|อาหาร"),
];
        //-------

    }
}
