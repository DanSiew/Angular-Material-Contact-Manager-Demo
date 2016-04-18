/// <binding BeforeBuild='moveToCss, moveToLibs, moveToSvg' AfterBuild='moveToCss, moveToHtml, moveToLibs, moveToScripts' />
/*
This file in the main entry point for defining Gulp tasks and using Gulp plugins.
Click here to learn more. http://go.microsoft.com/fwlink/?LinkId=518007
*/

var gulp = require('gulp'),
    concat = require('gulp-concat'),
    angularFilesort = require('gulp-angular-filesort');
var templateCache = require('gulp-angular-templatecache');

var del = require('del');

var paths = {
    npmSrc: "./node_modules/",
    webroot: "./wwwroot/",
    appSrc: "./app/",
    libTarget: "./wwwroot/libs/",
    scriptsTarget: "./wwwroot/app/",
    cssTarget: "./wwwroot/css/",
    fontTarget: "./wwwroot/fonts/",
    distTarget: "./wwwroot/dist/",
    svgTarget: "./wwwroot/svg/"
};

var libsToMove = [
   paths.npmSrc + '/angular/angular.js',
   paths.npmSrc + '/angular-animate/angular-animate.js',
   paths.npmSrc + '/angular-aria/angular-aria.js',
   paths.npmSrc + '/angular-material/angular-material.js',
   paths.npmSrc + '/angular-message/angular-message.js',
   paths.npmSrc + '/angular-material-icons/angular-material-icons.js'
];

var cssToMove = [
   paths.npmSrc + '/angular-material/angular-material.min.css',
   paths.npmSrc + '/angular-material-icons/angular-material-icons.css',
   paths.appSrc + '/src/css/app.css'
];

var scrsToMove = [
   paths.appSrc + '/dist/**/**/*.js',
   '!' + paths.appSrc + '/dist/_all.js'
];

gulp.task('clean', function () {
    return del([paths.scriptsTarget,
        paths.distTarget
    ]);
});

gulp.task('moveToLibs', function () {
    return gulp.src(libsToMove).pipe(gulp.dest(paths.libTarget));
});

gulp.task('moveToCss', function () {
    return gulp.src(cssToMove).pipe(gulp.dest(paths.cssTarget));
});

gulp.task('moveToHtml', function () {
    return gulp.src(paths.appSrc + '/src/**/**/**.html')
    .pipe(gulp.dest(paths.distTarget));
});

gulp.task('moveToSvg', function () {
    return gulp.src(paths.appSrc + '/svg/**/**/**.svg')
    .pipe(gulp.dest(paths.svgTarget));
});

gulp.task('moveToScripts', function () {
    return gulp.src(scrsToMove)
        .pipe(angularFilesort())
        .pipe(concat('app.js'))
        .pipe(gulp.dest(paths.scriptsTarget));
});


