angular.module("ngLocale", [], ["$provide", function($provide) {
var PLURAL_CATEGORY = {ZERO: "zero", ONE: "one", TWO: "two", FEW: "few", MANY: "many", OTHER: "other"};
$provide.value("$locale", {"DATETIME_FORMATS":{"MONTH":["januar","februar","marts","april","maj","juni","juli","august","september","oktober","november","december"],"SHORTMONTH":["jan.","feb.","mar.","apr.","maj","jun.","jul.","aug.","sep.","okt.","nov.","dec."],"DAY":["søndag","mandag","tirsdag","onsdag","torsdag","fredag","lørdag"],"SHORTDAY":["søn","man","tir","ons","tor","fre","lør"],"AMPMS":["f.m.","e.m."],"medium":"dd/MM/yyyy HH.mm.ss","short":"dd/MM/yy HH.mm","fullDate":"EEEE 'den' d. MMMM y","longDate":"d. MMM y","mediumDate":"dd/MM/yyyy","shortDate":"dd/MM/yy","mediumTime":"HH.mm.ss","shortTime":"HH.mm"},"NUMBER_FORMATS":{"DECIMAL_SEP":",","GROUP_SEP":".","PATTERNS":[{"minInt":1,"minFrac":0,"macFrac":0,"posPre":"","posSuf":"","negPre":"-","negSuf":"","gSize":3,"lgSize":3,"maxFrac":3},{"minInt":1,"minFrac":2,"macFrac":0,"posPre":"","posSuf":" \u00A4","negPre":"-","negSuf":" \u00A4","gSize":3,"lgSize":3,"maxFrac":2}],"CURRENCY_SYM":"kr"},"pluralCat":function (n) {  if (n == 1) {    return PLURAL_CATEGORY.ONE;  }  return PLURAL_CATEGORY.OTHER;},"id":"da"});
}]);