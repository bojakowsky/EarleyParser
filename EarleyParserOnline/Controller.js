/// <reference path="Scripts/angular.js" />
/// <reference path="Scripts/jquery-2.2.0.min.js" />
var earleyApp = angular.module("earleyModule", []);

var earleyController = function ($scope, $http) {
    $http({
        method: 'GET',
        url: '/EarleyWebService.asmx/InitialTest',
        contentType: "application/json; charset=utf-8",
        dataType: "json"
    })
    .then(function (response) {
        $scope.charts = response.data;
        //$scope.chart = $scope.charts.chart;
    });
    $scope.charts = [];
    $scope.indexes = [];
    $scope.rules = [
        //leftSide, rightSide
    ];

    $scope.history = [
        //[rules, rules, ...]
    ];

    $scope.historyIndex = 1;

    $scope.SetStateIndexes = function SetStateIndexes() {
        $scope.indexes = [];
        var counter = 0;
        for (var i = 0 ; i < $scope.charts.length; i++) {
            for (var j = 0 ; j < $scope.charts[i].chart.length; j++) {
                $scope.indexes.push(counter);
                counter++;
            }
        }
    }

    $scope.RemoveAll = function()
    {
        $scope.rules = [];
    }

    $scope.RemoveRule = function (rule) {
        //$scope.rules = $scope.rules.filter(function (element) {
        //    return element != rule;
        //});
        var index = $scope.rules.indexOf(rule);
        $scope.rules.splice(index, 1);
    }

    $scope.AddRule = function () {
        $scope.rules.push({ leftSide: $scope.ruleLeftTerm, rightSide: $scope.ruleRightTerm });
        $scope.ruleLeftTerm = "";
        $scope.ruleRightTerm = "";
        
        $("#ruleLeftTermInput").focus();
    }

    $scope.AddToHistory = function ()
    {
        $scope.history.push({ name: $scope.historyIndex, rule: $scope.rules, sentence: $scope.sentenceToParse })
        $scope.historyIndex++;
    }

    $scope.RemoveFromHistory = function (history) {
        var index = $scope.history.indexOf(history);
        $scope.history.splice(index, 1);
    }

    $scope.LoadRulesFromHistory = function(story)
    {
        var historyElement = $scope.history.find(function (element, index, array) {
            if (element.name == story.name)
                return element;
        });

        if (historyElement)
        {
            $scope.rules = historyElement.rule;
            $scope.sentenceToParse = historyElement.sentence;
            $scope.RunParse();
        }
    }

    $scope.RunParseAndAddToHistory = function ()
    {
        $scope.AddToHistory();
        $scope.RunParse();
    }

    $scope.RunParse = function ()
    {
        var dataToSend = [{rules: $scope.rules, sentence: $scope.sentenceToParse}];
        $http({
            method: 'POST',
            url: '/EarleyWebService.asmx/ParseSentence',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: ({ sentenceToParse: angular.toJson(dataToSend) }),
        }).then(function (response) {
            $scope.charts = angular.fromJson(response.data.d);
        });
    }

};

earleyApp.controller("earleyController", earleyController);