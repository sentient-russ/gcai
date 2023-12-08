"use strict";
var connection = new signalR.HubConnectionBuilder().withUrl("/posthub").build();
//Disable the send button until connection is established.
document.getElementById("sendButton").disabled = true;
document.getElementById("userInput").value = "Anonymous";
document.getElementById("userInput").disabled = true;
//this starts the indicator that the backend system is processing.
function startSpin() {
    const spinnerClass = document.querySelectorAll('.spinner');
    spinnerClass.forEach(spin => { spin.classList.remove('hidden') });
    var placeholder = document.getElementById("messageInputAI").placeholder = 'Please wait for your response to appear below.  I am thinking hard about it now...';
    document.getElementById("messageInputAI").disabled = true;
}
//this starts the indicator that the backend system is processing.
function stopSpin() {
    const spinnerClass = document.querySelectorAll('.spinner');
    spinnerClass.forEach(spinner => { spinner.classList.add('hidden') });
    var placeholder = document.getElementById("messageInputAI").placeholder = 'Enter your next AA related question here.';
    document.getElementById("messageInputAI").disabled = false;
}
//update total-contributions
function updateTotalCounts(newTotal) {
    const tags = document.querySelectorAll('.total-contributions');
    newTotal = newTotal;
    tags.forEach(tag => { tag.textContent = newTotal });
}
//clear button
function eraseText() {
    document.getElementById("messageInputTruth").value = "";
    document.getElementById("messageInputHumor").value = "";
    document.getElementById("messageInputProblem").value = "";
    document.getElementById("messageInputSolution").value = "";
    document.getElementById("messageInputAI").value = "";
}
//inbound message handling
connection.on("ReceiveMessageTruth", function (user, message, id, screenname, contributions) {
    document.getElementById("insert-messages").classList.add("messages-container");
    var div2 = document.createElement("div");
    div2.classList.add("user-message-container");
    div2.id = id;
    const loggedInScreename = document.querySelector(".screenname").id;
    /*----------------------------*/
    var div3 = document.createElement("div");
    div3.classList.add('avatar-row');
    div2.appendChild(div3);
    var div10 = document.createElement("div");
    div10.classList.add("avatar-column");
    div3.appendChild(div10);
    var div11 = document.createElement("div");
    div11.classList.add("avatar-row-user");
    div11.classList.add('padding-right-5');
    div10.appendChild(div11);
    var span1 = document.createElement('span');
    span1.textContent = `${screenname}`;
    div11.appendChild(span1);
    var div15 = document.createElement("div");
    div15.classList.add("user-info-container");
    div10.appendChild(div15);
    var div12 = document.createElement("div");
    div12.classList.add("avatar-row-user-count");
    div12.textContent = `${contributions}`;
    div12.classList.add('padding-right-5');
    if (screenname == loggedInScreename) {
        div12.classList.add("total-contributions");
        updateTotalCounts(contributions);
    }
    div15.appendChild(div12);
    var div16 = document.createElement("div");
    div16.classList.add("material-symbols-outlined");
    div16.classList.add("avatar-row-user-badge");
    div16.textContent = "social_leaderboard";
    div15.appendChild(div16);
    /*----------------------------*/
    var div13 = document.createElement("div");
    div13.classList.add("avatar-column");
    div3.appendChild(div13);
    var img1 = document.createElement('img');
    img1.classList.add('avatar-img');
    img1.src = "/img/user-avatar-filled.svg";
    div13.appendChild(img1);
    var div4 = document.createElement("div");
    div4.classList.add('message-row');
    div2.appendChild(div4);
    var h3 = document.createElement("h3");
    h3.textContent = `A Members Truth:`;
    div4.appendChild(h3);
    var p = document.createElement("p");
    p.textContent = `${message}`;
    h3.appendChild(p);
    var div4 = document.createElement('div');
    div4.classList.add('response-row');
    div2.appendChild(div4);
    var div5 = document.createElement('div');
    div5.classList.add("response-column1");
    div5.setAttribute("id", id);
    div4.appendChild(div5);
    var div6 = document.createElement('div');
    div6.classList.add("response-column2");
    div6.setAttribute("id", id);
    div4.appendChild(div6);
    var div7 = document.createElement('div');
    div7.classList.add("response-column5");
    div4.appendChild(div7);
    var div8 = document.createElement('div');
    div8.classList.add("response-column3");
    div8.setAttribute("id", id);
    div4.appendChild(div8);
    var div9 = document.createElement('div');
    div9.classList.add("response-column4");
    div9.setAttribute("id", id);
    div4.appendChild(div9);
    var img2 = document.createElement('img')
    img2.classList.add('thumbs-up-img');
    img2.classList.add('filter-green');
    img2.src = "/img/thumbs-up.svg";
    div5.appendChild(img2);
    var img3 = document.createElement('img')
    img3.classList.add('thumbs-dn-img');
    img3.classList.add('filter-red');
    img3.src = "/img/thumbs-down.svg";
    div6.appendChild(img3);
    var img4 = document.createElement('img')
    img4.classList.add('star');
    img4.classList.add('filter-gold');
    img4.src = "/img/star.svg";
    div8.appendChild(img4);
    var img5 = document.createElement('img')
    img5.classList.add('report');
    img5.classList.add('filter-red');
    img5.src = "/img/report.svg";
    div9.appendChild(img5);
    document.getElementById("insert-messages").prepend(div2);
    stopSpin();
    initButtonRow();
});

connection.on("ReceiveMessageHumor", function (user, message, id, screenname, contributions) {
    const loggedInScreename = document.querySelector(".screenname").id;
    document.getElementById("insert-messages").classList.add("messages-container");
    var div2 = document.createElement("div");
    div2.classList.add("user-message-container");
    div2.classList.add(id);
    /*----------------------------*/
    var div3 = document.createElement("div");
    div3.classList.add('avatar-row');
    div2.appendChild(div3);
    var div10 = document.createElement("div");
    div10.classList.add("avatar-column");
    div3.appendChild(div10);
    var div11 = document.createElement("div");
    div11.classList.add("avatar-row-user");
    div11.classList.add('padding-right-5');
    div10.appendChild(div11);
    var span1 = document.createElement('span');
    span1.textContent = `${screenname}`;
    div11.appendChild(span1);
    var div15 = document.createElement("div");
    div15.classList.add("user-info-container");
    div10.appendChild(div15)
    var div12 = document.createElement("div");
    div12.classList.add("avatar-row-user-count");
    div12.textContent = `${contributions}`;
    div12.classList.add('padding-right-5');
    if (screenname == loggedInScreename) {
        div12.classList.add("total-contributions");
        updateTotalCounts(contributions);
    }
    div15.appendChild(div12);
    var div16 = document.createElement("div");
    div16.classList.add("material-symbols-outlined");
    div16.classList.add("avatar-row-user-badge");
    div16.textContent = "social_leaderboard";
    div15.appendChild(div16);
    /*----------------------------*/
    var div13 = document.createElement("div");
    div13.classList.add("avatar-column");
    div3.appendChild(div13);
    var img1 = document.createElement('img');
    var div13 = document.createElement("div");
    div13.classList.add("avatar-column");
    div3.appendChild(div13);
    var img1 = document.createElement('img');
    img1.classList.add('avatar-img');
    img1.src = "/img/user-avatar-filled.svg";
    div13.appendChild(img1);
    var div4 = document.createElement("div");
    div4.classList.add('message-row');
    div2.appendChild(div4);
    var h3 = document.createElement("h3");
    h3.textContent = `A Members Humor:`;
    div4.appendChild(h3);
    var p = document.createElement("p");
    p.textContent = `${message}`;
    h3.appendChild(p);
    var div4 = document.createElement('div');
    div4.classList.add('response-row');
    div2.appendChild(div4);
    var div5 = document.createElement('div');
    div5.classList.add("response-column1");
    div5.setAttribute("id", id);
    div4.appendChild(div5);
    var div6 = document.createElement('div');
    div6.classList.add("response-column2");
    div6.setAttribute("id", id);
    div4.appendChild(div6);
    var div7 = document.createElement('div');
    div7.classList.add("response-column5");
    div4.appendChild(div7);
    var div8 = document.createElement('div');
    div8.classList.add("response-column3");
    div8.setAttribute("id", id);
    div4.appendChild(div8);
    var div9 = document.createElement('div');
    div9.classList.add("response-column4");
    div9.setAttribute("id", id);
    div4.appendChild(div9);
    var img2 = document.createElement('img')
    img2.classList.add('thumbs-up-img');
    img2.classList.add('filter-green');
    img2.src = "/img/thumbs-up.svg";
    div5.appendChild(img2);
    var img3 = document.createElement('img')
    img3.classList.add('thumbs-dn-img');
    img3.classList.add('filter-red');
    img3.src = "/img/thumbs-down.svg";
    div6.appendChild(img3);
    var img4 = document.createElement('img')
    img4.classList.add('star');
    img4.classList.add('filter-gold');
    img4.src = "/img/star.svg";
    div8.appendChild(img4);
    var img5 = document.createElement('img')
    img5.classList.add('report');
    img5.classList.add('filter-red');
    img5.src = "/img/report.svg";
    div9.appendChild(img5);
    document.getElementById("insert-messages").prepend(div2);
    stopSpin();
    initButtonRow();
});
connection.on("ReceiveMessageProblemSolution", function (user, problem, solution, id, screenname, contributions) {
    const loggedInScreename = document.querySelector(".screenname").id;
    document.getElementById("insert-messages").classList.add("messages-container");
    var div2 = document.createElement("div");
    div2.classList.add("user-message-container");
    div2.classList.add(id);
    /*----------------------------*/
    var div3 = document.createElement("div");
    div3.classList.add('avatar-row');
    div2.appendChild(div3);
    var div10 = document.createElement("div");
    div10.classList.add("avatar-column");
    div3.appendChild(div10);
    var div11 = document.createElement("div");
    div11.classList.add("avatar-row-user");
    div11.classList.add('padding-right-5');
    div10.appendChild(div11);
    var span1 = document.createElement('span');
    span1.textContent = `${screenname}`;
    div11.appendChild(span1);
    var div15 = document.createElement("div");
    div15.classList.add("user-info-container");
    div10.appendChild(div15);
    var div12 = document.createElement("div");
    div12.classList.add("avatar-row-user-count");
    div12.textContent = `${contributions}`;
    div12.classList.add('padding-right-5');
    if (screenname == loggedInScreename) {
        div12.classList.add("total-contributions");
        updateTotalCounts(contributions);
    }
    div15.appendChild(div12);
    var div16 = document.createElement("div");
    div16.classList.add("material-symbols-outlined");
    div16.classList.add("avatar-row-user-badge");
    div16.textContent = "social_leaderboard";
    div15.appendChild(div16);
    /*----------------------------*/
    var div13 = document.createElement("div");
    div13.classList.add("avatar-column");
    div3.appendChild(div13);
    var img1 = document.createElement('img');
    var div13 = document.createElement("div");
    div13.classList.add("avatar-column");
    div3.appendChild(div13);
    var img1 = document.createElement('img');
    img1.classList.add('avatar-img');
    img1.src = "/img/user-avatar-filled.svg";
    div13.appendChild(img1);
    var div4 = document.createElement("div");
    div4.classList.add('message-row');
    div2.appendChild(div4);
    var h3 = document.createElement("h3");
    h3.textContent = `A Members Question:`;
    div4.appendChild(h3);
    var p = document.createElement("p");
    p.textContent = `${problem}`;
    h3.appendChild(p);
    var h32 = document.createElement("h3");
    h32.textContent = `This Members Answer:`;
    h3.appendChild(h32);
    var p2 = document.createElement("p");
    p2.textContent = `${solution}`;
    h3.appendChild(p2);
    var div4 = document.createElement('div');
    div4.classList.add('response-row');
    div2.appendChild(div4);
    var div5 = document.createElement('div');
    div5.classList.add("response-column1");
    div5.setAttribute("id", id);
    div4.appendChild(div5);
    var div6 = document.createElement('div');
    div6.classList.add("response-column2");
    div6.setAttribute("id", id);
    div4.appendChild(div6);
    var div7 = document.createElement('div');
    div7.classList.add("response-column5");
    div4.appendChild(div7);
    var div8 = document.createElement('div');
    div8.classList.add("response-column3");
    div8.setAttribute("id", id);
    div4.appendChild(div8);
    var div9 = document.createElement('div');
    div9.classList.add("response-column4");
    div9.setAttribute("id", id);
    div4.appendChild(div9);
    var img2 = document.createElement('img')
    img2.classList.add('thumbs-up-img');
    img2.classList.add('filter-green');
    img2.src = "/img/thumbs-up.svg";
    div5.appendChild(img2);
    var img3 = document.createElement('img')
    img3.classList.add('thumbs-dn-img');
    img3.classList.add('filter-red');
    img3.src = "/img/thumbs-down.svg";
    div6.appendChild(img3);
    var img4 = document.createElement('img')
    img4.classList.add('star');
    img4.classList.add('filter-gold');
    img4.src = "/img/star.svg";
    div8.appendChild(img4);
    var img5 = document.createElement('img')
    img5.classList.add('report');
    img5.classList.add('filter-red');
    img5.src = "/img/report.svg";
    div9.appendChild(img5);
    document.getElementById("insert-messages").prepend(div2);
    stopSpin();
    initButtonRow();

});

connection.on("ReceiveAIResponse", function (user, query, response, id, screenname) {

    document.getElementById("insert-ai-response").classList.add("messages-container");
    var div2 = document.createElement("div");
    div2.classList.add("user-message-container");
    div2.classList.add(id);
    var div3 = document.createElement("div");
    div3.classList.add('avatar-row');
    div2.appendChild(div3);
    var span1 = document.createElement('span');
    span1.textContent = `${screenname}`;
    span1.classList.add('padding-right-5');
    div3.appendChild(span1);
    var img1 = document.createElement('img');
    img1.classList.add('avatar-img');
    img1.src = "/img/user-avatar-filled.svg";
    div3.appendChild(img1);
    var div4 = document.createElement("div");
    div4.classList.add('message-row');
    div2.appendChild(div4);
    var h3 = document.createElement("h3");
    h3.textContent = `Question:`;
    div4.appendChild(h3);
    var p = document.createElement("p");
    p.textContent = `${query}`;
    h3.appendChild(p);
    var h32 = document.createElement("h3");
    h32.textContent = `Response:`;
    h3.appendChild(h32);
    var p2 = document.createElement("p");
    p2.classList.add('ai-response');
    p2.id = "ai-response";
    p2.textContent = `${response}`;
    h3.appendChild(p2);
    var div4 = document.createElement('div');
    div4.classList.add('response-row');
    div2.appendChild(div4);
    var div5 = document.createElement('div');
    div5.classList.add("response-column1");
    div5.setAttribute("id", id);
    div4.appendChild(div5);
    var div6 = document.createElement('div');
    div6.classList.add("response-column2");
    div6.setAttribute("id", id);
    div4.appendChild(div6);
    var div7 = document.createElement('div');
    div7.classList.add("response-column5");
    div4.appendChild(div7);
    var div8 = document.createElement('div');
    div8.classList.add("response-column3");
    div8.setAttribute("id", id);
    div4.appendChild(div8);
    var div9 = document.createElement('div');
    div9.classList.add("response-column4");
    div9.setAttribute("id", id);
    div4.appendChild(div9);
    var img2 = document.createElement('img')
    img2.classList.add('thumbs-up-img');
    img2.classList.add('filter-green');
    img2.src = "/img/thumbs-up.svg";
    div5.appendChild(img2);
    var img3 = document.createElement('img')
    img3.classList.add('thumbs-dn-img');
    img3.classList.add('filter-red');
    img3.src = "/img/thumbs-down.svg";
    div6.appendChild(img3);
    var img4 = document.createElement('img')
    img4.classList.add('star');
    img4.classList.add('filter-gold');
    img4.src = "/img/star.svg";
    div8.appendChild(img4);
    var img5 = document.createElement('img')
    img5.classList.add('report');
    img5.classList.add('filter-red');
    img5.src = "/img/report.svg";
    div9.appendChild(img5);
    document.getElementById("insert-ai-response").prepend(div2);
    stopSpin();
    initButtonRow();
});
//update contributions when current user casts a vote
connection.on("UpdateContributions", function (user, updatedContributions) {
    var currentUser = document.querySelector('.jam').id;
    if (user == currentUser) {
        updateTotalCounts(updatedContributions);
    }
});

//outgoing message handling
connection.start().then(function () {
    document.getElementById("sendButton").disabled = false;
    let starting = 0;
    let ending = 10;
    connection.invoke("SendMessages", starting, ending);
}).catch(function (err) {
    return console.error(err.toString());
});

document.getElementById("sendButton").addEventListener("click", function (event) {
    var user = document.querySelector('.jam').id;
    var screenname = document.querySelector('.screenname').id;
    const tabs = document.querySelectorAll('.tab');
    startSpin();


    if (tabs[0].classList.contains('active')) {
        var message = document.getElementById("messageInputTruth").value;
        let postId = "0";
        connection.invoke("SendMessageTruth", user, message, postId, screenname).catch(function (err) {
            return console.error(err.toString());
        });
    }
    if (tabs[1].classList.contains('active')) {
        var message = document.getElementById("messageInputHumor").value;
        let postId = "0";
        connection.invoke("SendMessageHumor", user, message, postId, screenname).catch(function (err) {
            return console.error(err.toString());
        });
    }
    if (tabs[2].classList.contains('active')) {
        var problem = document.getElementById("messageInputProblem").value;
        var solution = document.getElementById("messageInputSolution").value;
        let postId = "0";
        connection.invoke("SendMessageProblemSolution", user, problem, solution, postId, screenname).catch(function (err) {
            return console.error(err.toString());
        });
    }
    event.preventDefault();
    eraseText();
});
function aiNotification() {

    $.toast({
        text: 'This website cannot be trusted to give sound advice. AI is in its infancy and we suggest checking with your local AA group, sponsor, and healthcare professional before making use of any information you find here.',
        heading: 'Warning',
        showHideTransition: 'slide',
        hideAfter: 15000,
        position: 'bottom-center',
        icon: 'warning',
        allowToastClose: true,
    })
    $.toast({
        text: 'Currently I am a little slow.  Please be patient!',
        heading: 'Thinking through it now....',
        showHideTransition: 'slide',
        hideAfter: 20000,
        position: 'bottom-center',

    })

}
document.getElementById("sendButton2").addEventListener("click", function (event) {
    document.getElementById("testid").scrollIntoView();
    var user = document.querySelector('.jam').id;
    var screenname = document.querySelector('.screenname').id;
    const tabs = document.querySelectorAll('.tab');
    startSpin();
    aiNotification();
    console.log('Should be spinning');
    if (tabs[3].classList.contains('active')) {
        var queryAI = document.getElementById("messageInputAI").value;
        let postId = "0";
        connection.invoke("ProccessAI", user, queryAI, postId, screenname).catch(function (err) {
            return console.error(err.toString());
        });
    }

    event.preventDefault();
    eraseText();
});

//post response buttons
function initButtonRow() {
    var screenname = document.querySelector('.screenname').id;
    var pepper = document.querySelector('.pepper').id
    var jam = document.querySelector('.jam').id

    var col1 = document.querySelector('.response-column1');
    col1.onclick = function () {
        loginNotification();
        console.log('Thumbs Up');
        console.log(pepper.toString());
        console.log(jam.toString());
        console.log(col1.id);
        var voteTypeOut = "Up";
        var pepperOut = pepper.toString();
        var userNameOut = jam.toString();
        var messageIdOut = col1.id;
        var screenNameOut = screenname;


        connection.invoke("CastVote", voteTypeOut, pepperOut, userNameOut, messageIdOut, screenNameOut).catch(function (err) {
            return console.error(err.toString());
        });
    }

    var col2 = document.querySelector('.response-column2');
    col2.onclick = function () {
        loginNotification();
        console.log('Thumbs Down');
        console.log(pepper.toString());
        console.log(jam.toString());
        console.log(col2.id);

        var voteTypeOut = "Down";
        var pepperOut = pepper.toString();
        var userNameOut = jam.toString();
        var messageIdOut = col2.id;
        var screenNameOut = screenname;

        connection.invoke("CastVote", voteTypeOut, pepperOut, userNameOut, messageIdOut, screenNameOut).catch(function (err) {
            return console.error(err.toString());
        });

    }

    var col3 = document.querySelector('.response-column3');
    col3.onclick = function () {
        loginNotification();
        console.log('Stars');
        console.log(pepper.toString());
        console.log(jam.toString());
        console.log(col3.id);

        var voteTypeOut = "Star";
        var pepperOut = pepper.toString();
        var userNameOut = jam.toString();
        var messageIdOut = col3.id;
        var screenNameOut = screenname;

        connection.invoke("CastVote", voteTypeOut, pepperOut, userNameOut, messageIdOut, screenNameOut).catch(function (err) {
            return console.error(err.toString());
        });
    }

    var col4 = document.querySelector('.response-column4');
    col4.onclick = function () {
        loginNotification();
        console.log('Flagged');
        console.log(pepper.toString());
        console.log(jam.toString());
        console.log(col4.id);

        var voteTypeOut = "Flag";
        var pepperOut = pepper.toString();
        var userNameOut = jam.toString();
        var messageIdOut = col4.id;
        var screenNameOut = screenname;

        connection.invoke("CastVote", voteTypeOut, pepperOut, userNameOut, messageIdOut, screenNameOut).catch(function (err) {
            return console.error(err.toString());
        });
    }

    // Toast functions
    function loginNotification() {
        var element = document.querySelector(".jam").id;
        if (element == 'Anonymous') {
            $.toast({
                heading: 'Login Required',
                text: 'It is required that you create an account and login to vote',
                icon: 'info',
                loader: true,
                stack: 4,
                position: 'bottom-center',
                hideAfter: 4000,
            })
        } else {
            $.toast({
                heading: 'Submitted',
                text: 'Success! Note: One vote per user, per post.',
                icon: 'success',
                loader: true,
                stack: 4,
                position: 'bottom-center',
                hideAfter: 4000,
            })
        }
    }
    let user = document.querySelector('.username').id;

    var aiResponse = document.querySelector(".ai-response");
    $(aiResponse).typewrite({
        'delay': 25, //time in ms between each letter
        'extra_char': '', //"cursor" character to append after each display
        'trim': true, // Trim the string to type (Default: false, does not trim)
        'callback': null // if exists, called after all effects have finished
    });

}

//clear button
function eraseText() {
    document.getElementById("messageInputTruth").value = "";
    document.getElementById("messageInputHumor").value = "";
    document.getElementById("messageInputProblem").value = "";
    document.getElementById("messageInputSolution").value = "";
    document.getElementById("messageInputAI").value = "";
}
//make sure AI messages are hidden on page reload
const aiMessaagesClass = document.querySelectorAll('.ai-container');
aiMessaagesClass.forEach(elem => { elem.classList.add('hidden') });
//hide or reveal messages
function hidePostMessages() {
    const postMessaagesClass = document.querySelectorAll('.messages-container');
    postMessaagesClass.forEach(elem => { elem.classList.add('hidden') });
}
function revealPostMessages() {
    const postMessaagesClass = document.querySelectorAll('.messages-container');
    postMessaagesClass.forEach(elem => { elem.classList.remove('hidden') });
}
function hideAIMessages() {
    const aiMessaagesClass = document.querySelectorAll('.ai-container');
    aiMessaagesClass.forEach(elem => { elem.classList.add('hidden') });
}
function revealAIMessages() {
    const aiMessaagesClass = document.querySelectorAll('.ai-container');
    aiMessaagesClass.forEach(elem => { elem.classList.remove('hidden') });
    invokeIAHello();
}
//Invokes initial AI message from the server.
function invokeIAHello() {
    document.getElementById("testid").scrollIntoView();
    var user = document.querySelector('.jam').id;
    var screenname = document.querySelector('.screenname').id;
    connection.invoke("helloAI", user, screenname).catch(function (err) {
        return console.error(err.toString());
    });
}
//tabs handling
const tabs = document.querySelectorAll('.tab');
const indicators = document.querySelectorAll('.indicator');
const contents = document.querySelectorAll('.content');

contents.forEach(content => { content.classList.remove('hide_textarea') });
contents[1].classList.add('hide_textarea');
contents[2].classList.add('hide_textarea');
contents[3].classList.add('hide_textarea');

contents[0].classList.add('active');
indicators[0].classList.add('active');
tabs[0].classList.add('active');

tabs.forEach((tab, index) => {
    tab.addEventListener('click', (e) => {
        eraseText();
        tabs.forEach(tab => { tab.classList.remove('active') });
        indicators.forEach(indicator => { indicator.classList.remove('active') });
        contents.forEach(content => { content.classList.remove('active') });

        if (tab.classList[0].includes('tab1')) {
            indicators.forEach(indicator => { indicator.classList.remove('active') });
            indicators[0].classList.add('active');
            tabs[0].classList.add('active');
            contents.forEach(content => { content.classList.remove('active') });
            contents.forEach(content => { content.classList.remove('hide_textarea') });
            contents[1].classList.add('hide_textarea');
            contents[2].classList.add('hide_textarea');
            contents[3].classList.add('hide_textarea');
            contents[4].classList.add('hide_textarea');
            contents[0].classList.add('active');

        }
        if (tab.classList[0].includes('tab2')) {
            indicators.forEach(indicator => { indicator.classList.remove('active') });
            indicators[1].classList.add('active');
            tabs[1].classList.add('active');
            contents.forEach(content => { content.classList.remove('active') });
            contents.forEach(content => { content.classList.remove('hide_textarea') });
            contents[0].classList.add('hide_textarea');
            contents[2].classList.add('hide_textarea');
            contents[3].classList.add('hide_textarea');
            contents[4].classList.add('hide_textarea');
            contents[1].classList.add('active');

        }
        if (tab.classList[0].includes('tab3')) {
            indicators.forEach(indicator => { indicator.classList.remove('active') });
            indicators[2].classList.add('active');
            tabs[2].classList.add('active');
            contents.forEach(content => { content.classList.remove('active') });
            contents.forEach(content => { content.classList.remove('hide_textarea') });
            contents[0].classList.add('hide_textarea');
            contents[1].classList.add('hide_textarea');
            contents[4].classList.add('hide_textarea');
            contents[2].classList.add('active');
            contents[3].classList.add('active');

        }
        if (tab.classList[0].includes('tab4-ai')) {
            indicators.forEach(indicator => { indicator.classList.remove('active') });
            indicators[3].classList.add('active');
            tabs[3].classList.add('active');
            contents.forEach(content => { content.classList.remove('active') });
            contents.forEach(content => { content.classList.remove('hide_textarea') });
            contents[0].classList.add('hide_textarea');
            contents[1].classList.add('hide_textarea');
            contents[2].classList.add('hide_textarea');
            contents[3].classList.add('hide_textarea');
            contents[4].classList.add('active');
            tabs[3].classList.add('active');
            
        }
    })
})
//end tabs

//begin card flip
const toggle_button = document.querySelector('.on-off-toggle__input');
const card = document.getElementById('card__inner');

toggle_button.addEventListener('click', function () {
    eraseText();

    card.classList.toggle('is--flipped');
    if (card.classList.contains('is--flipped')) {
        revealAIMessages();
        hidePostMessages();
        indicators.forEach(indicator => { indicator.classList.remove('active') });
        indicators[3].classList.add('active');

        contents.forEach(content => { content.classList.remove('active') });
        contents.forEach(content => { content.classList.remove('hide_textarea') });
        contents[0].classList.add('hide_textarea');
        contents[1].classList.add('hide_textarea');
        contents[2].classList.add('hide_textarea');
        contents[3].classList.add('hide_textarea');
        contents[4].classList.add('active');
        tabs[3].classList.add('active');

    }
    if (!card.classList.contains('is--flipped')) {
        revealPostMessages();
        hideAIMessages();
        indicators.forEach(indicator => { indicator.classList.remove('active') });
        indicators[0].classList.add('active');
        contents.forEach(content => { content.classList.remove('active') });
        contents.forEach(content => { content.classList.remove('hide_textarea') });
        contents[1].classList.add('hide_textarea');
        contents[2].classList.add('hide_textarea');
        contents[3].classList.add('hide_textarea');
        contents[4].classList.add('hide_textarea');
        contents[0].classList.add('active');
    }
});
//end card flip



