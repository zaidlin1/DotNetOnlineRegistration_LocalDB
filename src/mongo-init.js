db = db.getSiblingDB('admin');
// move to the admin db - always created in Mongo
//db.auth("rootUser", "rootPassword");
// log as root admin if you decided to authenticate in your docker-compose file...
db = db.getSiblingDB('CoursesDb');
// create and move to your new database
//db.createUser({
//    'user': "dbUser",
//    'pwd': "dbPwd",
//    'roles': [{
//        'role': 'dbOwner',
//        'db': 'DB_test'
//    }]
//});
// user created
db.createCollection('Courses');
// add new collection