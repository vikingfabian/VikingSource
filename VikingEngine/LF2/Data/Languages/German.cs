using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VikingEngine.LF2.Data.Languages
{
    //class German : AbsLanguage
    //{
    //    override public string Resume() { return "Weiter spielen"; }
    //    override public string Use() { return "Verwendunge "; }
    //    override public string MenuStartGame() { return "Spiel Starten"; }
    //    override public string MenuControlScheme() { return "Kontrollen Überblick"; }
    //    override public string MenuCredits() { return "Sponsorenliste"; }
    //    override public string MenuExitGame() { return "Spiel Verlassen"; }

    //    override public string MenuPressSTART() { return "Anfangen zu spielen"; }
    //    override public string MenuXtoSwapSplit() { return "Split richtung"; }
    //    override public string MenuStartTrial() { return "Beginnen Testmodus"; }

    //    override public string ThisIsTrial() { return "Du bist in Testmodus"; }
    //    override public string NoBuyInTrial() { return "Sie können nicht kaufen Begriffe im Testmodus"; }
    //    override public string BuyLootfest() { return "Kaufen dieses Spiel (80p)"; } //skriv (80p) i alla övriga språk

    //    override public string MenuAtoJoin() { return "Beitreten"; } //A drücken, um beitreten//
    //    override public string CreditsProgrammer() { return "Programmierer"; }
    //    override public string CreditsFrenchTranslation() { return "Französisch-Übersetzung"; }
    //    override public string CreditsPlaytesters() { return "Main Testspieler"; }
    //    override public string CreditsThanksXNA() { return "Ein großes Dankeschön an die Testspieler bei XNA App Hub"; }
    //    override public string CreditsFeedbackTitle() { return "Kontakt"; }
    //    override public string CreditsFeedback() { return "Wir begrüßen Fragen und Kritik über dieses Spiel, oder Anregungen für zukünftige Versionen"; }
        
    //    override public string ErrNotSignedInTitle() { return "Nicht angemeldet"; }//
    //    override public string ErrNotSignedInDesc() { return "Sie müssen sich anmelden, um ein Profil"; }
    //    override public string ErrNoLiveTitle() { return "Keine Verbindung zu Live gefunden"; }
    //    override public string ErrNoLiveDesc() { return "Sie sind verpflichtet, mit Xbox Live verbunden sein"; }
    //    override public string ControlsAttack() { return "attackieren"; }
    //    override public string ControlsUseBow() { return "Verwendung Bogen"; }
    //    override public string ControlsEatPie() { return "Apfelkuchen Essen, zur Wiederherstellung der Gesundheit"; }
    //    override public string ControlsPause() { return "Pause Menü"; }
    //    override public string ControlsMap() { return "LandKarte ansehen"; }
    //    override public string ControlsZoom() { return "Zoomen"; }
    //    override public string ControlsBirdInInventory() { return "Geben Sie inventar durch das Pause-Menü, um Ihre Nutzung Falke oder Taube"; }
    //    override public string ControlsPlayMusic() { return "Musik spielen"; }
    //    override public string ControlsNextSong() { return "Nächsten Song"; }

    //    static readonly List<string> Welcome = new List<string> 
    //    {
    //        "Willst du auf Opas Knie sitzen",
    //        "Hallo, mein jungen!",
    //        "Schön, dich wieder zu sehen!"
    //    };
    //    override public string GranpaWelcomePhrase() { return Welcome[Ref.rnd.Int(Welcome.Count)]; }
    //    override public string GranpaFirstTimeWelcome1() { return "Hallo mein Sohn! wollen ein paar Süßigkeiten?"; }
    //    override public string Next() { return "Weiter"; }
    //    override public string GranpaFirstTimeWelcome2() { return "Fürchte dich nicht mit mir über deine Suche zu bitten. Ich werde hier bleiben in Ihrem Dorf."; }
    //    override public string GranpaWhatsNextLink() { return "Was kommt als nächstes?"; }
    //    override public string GranpaStoryLink() { return "Historie"; }
    //    override public string GranpaStory() { return "Dieses Land durch drei böse Bosse ist terrorisiert, bietet der König eine große Belohnung für den, der sie besiegt."; }
    //    override public string GranpaControlsJoke1() { return "Kontrollen? Was ist das? Dieses Spiel ist unter sich so zu ernst, um die vierte Wand zu brechen!"; }
    //    override public string GranpaControlsJoke2() { return "Haha, just kidding!"; } //not translated
    //    override public string GranpaControlsNoBow() { return "Sie müssen einen Bogen zu kaufen"; }
    //    override public string GranpaMission1() { return "Sie müssen sparen Geld für einen Bogen. Sie können eine von der Verkäufer. Du wirst ihn in der linken oberen Ecke des Dorfes zu finden."; }
    //    override public string GranpaMission2() { return "Mit dem Falken auf die nächste böse Chef zu finden"; }
    //    override public string GranpaGameOver() { return "Mein Gott! Was machst du hier? Das Spiel ist beendet! Schalten Sie die Xbox und erhalten mit Ihrem Leben abgeschlossen haben!"; }
    //    static readonly List<string> SalesmanWelcome = new List<string> 
    //    {
    //        "Nur die besten Waren",
    //        "Willkommen, mein Herr",
    //        "Lassen Sie mich Ihnen helfen, die diesen schweren goldenen Beutel",
    //        "Was möchten Sie kaufen?",
    //    };
    //    override public string SalesmanWelcomePhrase() { return SalesmanWelcome[Ref.rnd.Int(SalesmanWelcome.Count)]; }
    //    override public string SaleNeedMoreGold(int amountNeeded)
    //    {
    //        return " Man muss " + amountNeeded.ToString() + "g haben, zu diesem kaufen";
    //    }
    //    override public string SaleDontBuy() { return "Nicht kaufen"; }
    //    override public string SaleBuy() { return "Kaufen"; }
    //    override public string NameSalesman() { return "Verkäufer"; }
    //    override public string NameGrandfather() { return "Okel"; } //Whatever sweet name on a older man you can find
    //    override public string Shield() { return "Schild"; }
    //    override public string Sword() { return "Schwert"; }
    //    override public string Bow() { return "Bogen"; }
    //    override public string Pigeon() { return "Taube"; }
        
    //    override public string Hawk() { return "Falke"; }
    //    override public string Level() { return "lvl"; }
    //    override public string SaleShieldDesc(int level)
    //    {
    //        switch (level)
    //        {
    //            default:
    //                return "Will bounce off projectiles, that comes straight in front of you";
    //            case 1:
    //                return "Will bounce off projectiles, from the front and sides.";
    //            case 2:
    //                return "Will bounce off projectiles and turn them against your enemy";
    //        }
    //    }
    //    override public string SaleSwordDesc() 
    //    { return "Eine größere und mächtigere Schwert"; }
    //    override public string SaleBowDesc(int level)
    //    {
    //        if (level == 0)
    //            return "Sie müssen es haben, zum ersten Boss besiegen";
    //        else
    //            return "Man muss den Bogen haben, zum nächsten Boss zu besiegen";
    //    }
    //    override public string Arrows() { return "Pfeile"; }
    //    override public string ApplePie() { return "Apfelkuchen"; }
    //    override public string SaleBirdAccess(string birdName)
    //    { return  "Greifen Sie auf Ihre " + birdName + " im Pause-Menü durch Drücken von Start"; }
    //    override public string SaleRBtoEat() { return  "Presse RB zu essen den Kuchen"; }
    //    override public string SaleArrowsDesc() { return "Munition, um den Bogen"; }
    //    override public string SaleHawkDesc() { return "Zeigt Ihnen, wo der nächste Chef ist versteckt"; }
    //    override public string SalePigeonDesc() { return "Wird der nächste Dorf fliegen" ; }
    //    override public string SalePieDesc() { return "Will Wiederherstellung Ihrer Gesundheit"; }

    //    override public string CamAngle() { return "Winkel"; }
    //    override public string CamAngleTop() { return "Oben winkel"; }
    //    override public string CamAngleTilted() { return "gekippt"; }
    //    override public string CamAngleSide() { return "Seite winkel"; }
    //    override public string CamActive() { return "Active Camera"; }
    //    override public string LoadingTerrain() { return "Einlegen Terrain ..."; }
    //    override public string LoadingMap() { return "Einlegen Karte"; }
    //    override public string ExitGame() { return "Schließen spiel?"; }
    //    override public string ExitGameDesc() { return "Aller Fortschritt geht verloren"; }
    //    override public string ExitGameYes() { return "Beenden!"; }
    //    override public string ExitGameNo() { return "Mehr spielen"; }
    //    override public string DeathTitle() { return  "Sie starb"; }
    //    override public string DeathDesc() { return  "Du wirst alles zu halten, außer dass Sie die Hälfte Ihres Gold verlieren wird."; }
    //    override public string DeathContinue() { return "Weiter Spielen?"; }
    //    override public string DeathContinueYes() { return "Weitermachen!"; }
    //    override public string DeathContinueNo() { return "Exit spiel"; }
    //    override public string GameWonTitle() { return "Herzlichen Glückwunsch!"; }
    //    override public string GameWonText() { return "Sie haben das Böse Chef besiegt,  bekam das halbe Königreich  und Sie wollen bums.. Ich meine, die Prinzessin heiraten. Bla bla bla, Sie kennen die Geschichte, sehen Sie in Lootfest2!"; }
    //    override public string LostControllerTitle() { return "Controller getrennt"; }
    //    override public string LostControllerFriend() { return "Warten Sie Ihren Freund, zu verbinden seine Controller."; }
    //    override public string LostControllerIgnoreFriend() { return "Ignorieren"; }
    //    override public string AppearanceHatTitle() { return "Hut"; }
    //    override public string AppearanceFacialTitle() { return "Gesichtsbehaarung"; }
    //    override public string AppearanceSkinnTitle() { return "Skinn Farbe"; }
    //    override public string AppearanceClothColTitle() { return "Tunika Farbe"; }
    //    override public string AppearanceHairColTitle() { return "Haarfarbe"; }
    //    public override string AppearanceHairNon()
    //    {
    //        return "Keine haar";
    //    }
    //    override public string AppearanceHatNon() { return "Keine Hut"; }
    //    override public string AppearanceHatVendel() { return "Vendel helm"; }
    //    override public string AppearanceHatHorned() { return "Gehörnt helm"; }
    //    override public string AppearanceHatKnight() { return "Ritterhelm"; }
    //    override public string AppearanceFacialNon() { return "Keine haar"; }
    //    override public string AppearanceFacialSmallBeard() { return "Kleine bart"; }
    //    override public string AppearanceFacialLargeBeard() { return "Große bart"; }
    //    override public string AppearanceFacialPlumbersMustache() { return "Installateur Schnurrbart"; }
    //    override public string AppearanceFacialBikersMustache() { return "Motorradfahrers Schnurrbart"; }
    //    override public string GameMenuInventory() { return "Inventar"; }
    //    override public string GameMenuOpenMap() { return "Offene karte"; }
    //    override public string GameMenuAppearance() { return "Erscheinungsbildes"; }
    //    override public string GameMenuCameraSettings() { return "Kamera-Einstellungen"; }
    //    override public string ClothColorName(Players.ClothColor col)
    //    {
    //        switch (col)
    //        {
    //            case Players.ClothColor.Blue:
    //                return "Blau";
    //            case Players.ClothColor.DarkBlue:
    //                return "Dunkelblau";
    //            case Players.ClothColor.Gray:
    //                return "Grau";
    //            case Players.ClothColor.Green:
    //                return "Grün";
    //            case Players.ClothColor.Red:
    //                return "Rot";
    //            case Players.ClothColor.Yellow:
    //                return "Gelb";

    //        }
    //        return TextLib.EmptyString;
    //    }
    //    override public string SkinnColorName(Players.SkinnColor col)
    //    {
    //        switch (col)
    //        {
    //            case Players.SkinnColor.Dark:
    //                return "Dunkler";
    //            case Players.SkinnColor.Pink:
    //                return "Rosa";
    //            case Players.SkinnColor.White:
    //                return "Weiß";

    //        }
    //        return TextLib.EmptyString;
    //    }
    //    override public string HairColorName(Players.HairColor col)
    //    {
    //        switch (col)
    //        {
    //            case Players.HairColor.Black:
    //                return "Schwarz";
    //            case Players.HairColor.Brown:
    //                return "Braun";
    //            case Players.HairColor.NoHair:
    //                return "Keine haar";
    //            case Players.HairColor.RedBrown:
    //                return "Rot braun";
    //            case Players.HairColor.White:
    //                return "Weiß";

    //        }
    //        return TextLib.EmptyString;
    //    }
    //}
}
