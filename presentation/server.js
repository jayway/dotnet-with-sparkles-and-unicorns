const connect = require('connect');
const static = require('serve-static');
const liverreload = require('livereload');

var csrv = connect();

csrv.use(static('.'));
csrv.listen(1337);

const lrsrv = liverreload.createServer({
    exts: [
        'html',
        'css',
        'js',
        'md'
    ]
});
lrsrv.watch('.');

