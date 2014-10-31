"define metadata";
({
    "description": "RiSC-16 ASM syntax highlighter",
    "dependencies": {
        "syntax_manager": "0.0"
    },
    "provides": [
        {
            "ep": "syntax",
            "name": "asm",
            "pointer": "#ASMSyntax",
            "fileexts": [ "asm" ]
        }
    ]
});
"end";

var SC = require('sproutcore/runtime').SC;
var Promise = require('bespin:promise').Promise;
var StandardSyntax = require('syntax_manager:controllers/standardsyntax').
    StandardSyntax;

exports.ASMSyntax = StandardSyntax.create({
    states: {
        start: [
            {
                regex:  /^(?:break|case|catch|continue|default|delete|do|else|false|finally|for|function|if|in|instanceof|let|new|null|return|switch|this|throw|true|try|typeof|var|void|while|with)(?![a-zA-Z0-9_])/,
                tag:    'keyword'
            },
            {
                regex:  /^[A-Za-z_][A-Za-z0-9_]*/,
                tag:    'identifier'
            },
            {
                regex:  /^[^'"\/ \tA-Za-z0-9_]+/,
                tag:    'plain'
            },
            {
                regex:  /^[ \t]+/,
                tag:    'plain'
            },
            {
                regex:  /^'/,
                tag:    'string',
                then:   'qstring'
            },
            {
                regex:  /^"/,
                tag:    'string',
                then:   'qqstring'
            },
            {
                regex:  /^\/\/.*/,
                tag:    'comment'
            },
            {
                regex:  /^\/\*/,
                tag:    'comment',
                then:   'comment'
            },
            {
                regex:  /^./,
                tag:    'plain'
            }
        ],

        qstring: [
            {
                regex:  /^'/,
                tag:    'string',
                then:   'start'
            },
            {
                regex:  /^(?:\\.|[^'\\])+/,
                tag:    'string'
            }
        ],

        qqstring: [
            {
                regex:  /^"/,
                tag:    'string',
                then:   'start'
            },
            {
                regex:  /^(?:\\.|[^"\\])+/,
                tag:    'string'
            }
        ],

        comment: [
            {
                regex:  /^[^*\/]+/,
                tag:    'comment'
            },
            {
                regex:  /^\*\//,
                tag:    'comment',
                then:   'start'
            },
            {
                regex:  /^[*\/]/,
                tag:    'comment'
            }
        ]
    }
});
