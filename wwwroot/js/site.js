﻿"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/posthub").build();
//Disable the send button until connection is established.
document.getElementById("sendButton").disabled = true;
document.getElementById("userInput").value = "Anonymous";
document.getElementById("userInput").disabled = true;


//Hides everything above the input box
function hideBanner() {
    var banner_div_block = document.getElementById('banner-fade');
    if (banner_div_block.classList.contains('banner-fade-hidden')) { } else { banner_div_block.classList.add('banner-fade-hidden'); }
    let endThis = document.getElementById('banner-fade');
    endThis.addEventListener('animationstart', (ev) => {
        window.scrollTo({
            behavior: 'smooth',
            top: endThis.offsetTop + endThis.clientHeight - window.innerHeight
        });
    });
    //ensures placement regardless of window position at execution.  //eliminates overshooting the mark
    endThis.addEventListener('animationend', (ev) => {
        window.scrollTo({
            behavior: 'smooth',
            top: endThis.offsetTop + endThis.clientHeight - window.innerHeight
        });
    });
    endThis.classList.add('hidden');
}

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
//inbound message handling cheet sheet
/*                    
List Order
[0] = PostId
[1] = Truth
[2] = Humor
[3] = Problem
[4] = Solution
[5] = Email/Username
[6] = ScreenName
[7] = TotalUserContribution
[8] = TotalPostUpVotes
[9] = TotalPostDownVotes
[10] = TotalPostFlags
[11] = UserUpVoted
[12] = UserDownVoted
[13] = UserFavorited
[14} = UserFlagged
*/
connection.on("ReceiveMessageTruth", function (postDataIn) {
    var postData = JSON.parse(postDataIn);
    var postId = postData[0];
    var truth = postData[1];
    var humor = postData[2];
    var problem = postData[3];
    var solution = postData[4];
    var username = postData[5];
    var screenname = postData[6];
    var totalUserContributions = postData[7];
    var totalPostUpVotes = postData[8];
    var totalPostDownVotes = postData[9];
    var totalPostFlags = postData[10];
    var userUpVoted = postData[11];
    var userDownVoted = postData[12];
    var userFavorited = postData[13];
    var userFlagged = postData[14];

    const loggedInScreename = document.querySelector(".screenname").id;
    document.getElementById("insert-messages").classList.add("messages-container");
    var div2 = document.createElement("div");
    div2.classList.add("user-message-container");
    div2.classList.add(postId);
    div2.setAttribute("id", postId);
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
    div12.textContent = `${totalUserContributions}`;
    div12.classList.add('padding-right-5');
    if (screenname == loggedInScreename) {
        div12.classList.add("total-contributions");
        updateTotalCounts(totalUserContributions);
    }
    div15.appendChild(div12);
    var div16 = document.createElement("div");
    div16.classList.add("material-symbols-outlined");
    div16.classList.add("avatar-row-user-badge");
    div16.textContent = "social_leaderboard";
    div15.appendChild(div16);
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
    var div14 = document.createElement("div");
    div14.classList.add('message-colum');
    div2.appendChild(div14);
    var div4 = document.createElement("div");
    div4.classList.add('message-row');
    div14.appendChild(div4);
    var h3 = document.createElement("h3");
    h3.classList.add('message-h3-heading');
    h3.textContent = `Truth:`;
    div4.appendChild(h3);
    var div16 = document.createElement("div");
    div16.classList.add('message-row');
    div2.appendChild(div16);
    var p = document.createElement("p");
    p.classList.add('member-question');
    p.classList.add('message-paragraph');
    p.textContent = `${truth}`;
    div16.appendChild(p);
    var div4 = document.createElement('div');
    div4.classList.add('response-row');
    div2.appendChild(div4);
    var div5 = document.createElement('div');
    div5.classList.add("response-column1");
    if (userUpVoted.localeCompare("1") == 0) { div5.classList.add("user-voted"); }
    div5.setAttribute("id", postId);
    div4.appendChild(div5);
    var div10 = document.createElement('div');
    div10.classList.add("vote-count");
    var span10 = document.createElement('span');
    span10.textContent = `${totalPostUpVotes}`;
    span10.setAttribute("id", postId + "-usr_upVoted");
    div10.appendChild(span10);
    div4.appendChild(div10);
    var div6 = document.createElement('div');
    div6.classList.add("response-column2");
    if (userDownVoted.localeCompare("1") == 0) { div6.classList.add("user-voted"); }
    div6.setAttribute("id", postId);
    div4.appendChild(div6);
    var div11 = document.createElement('div');
    div11.classList.add("vote-count");
    var span11 = document.createElement('span');
    span11.textContent = `${totalPostDownVotes}`;
    span11.setAttribute("id", postId + "-usr_downVoted");
    div11.appendChild(span11);
    div4.appendChild(div11);
    var div7 = document.createElement('div');
    div7.classList.add("response-column5");
    div4.appendChild(div7);
    var div8 = document.createElement('div');
    div8.classList.add("response-column3");
    if (userFavorited.localeCompare("1") == 0) { div8.classList.add("user-voted"); }
    div8.setAttribute("id", postId);
    div4.appendChild(div8);
    var div12 = document.createElement('div');
    div12.classList.add("transparent");
    div12.classList.add("vote-count");
    var span12 = document.createElement('span');
    span12.setAttribute("id", postId + "-usr_stared");
    span12.textContent = "0";
    div12.appendChild(span12);
    div4.appendChild(div12);
    if (userFavorited == "1") {
        span12.textContent = "1";
        div12.classList.remove("transparent");
    }
    var div9 = document.createElement('div');
    div9.classList.add("response-column4");
    if (userFlagged.localeCompare("1") == 0) { div9.classList.add("user-voted"); }
    div9.setAttribute("id", postId);
    div4.appendChild(div9);
    var div13 = document.createElement('div');
    div13.classList.add("vote-count");
    var span13 = document.createElement('span');
    span13.textContent = `${totalPostFlags}`;
    span13.setAttribute("id", postId + "-usr_flagged");
    div13.appendChild(span13);
    div4.appendChild(div13);
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
    img4.classList.add('star-img');
    img4.classList.add('filter-gold');
    img4.src = "/img/star.svg";
    div8.appendChild(img4);
    var img5 = document.createElement('img')
    img5.classList.add('report-img');
    img5.classList.add('filter-red');
    img5.src = "/img/report.svg";
    div9.appendChild(img5);
    document.getElementById("insert-messages").prepend(div2);
    stopSpin();
    initButtonRow();
});
connection.on("ReceiveMessageHumor", function (postDataIn) {
    var postData = JSON.parse(postDataIn);
    var postId = postData[0];
    var truth = postData[1];
    var humor = postData[2];
    var problem = postData[3];
    var solution = postData[4];
    var username = postData[5];
    var screenname = postData[6];
    var totalUserContributions = postData[7];
    var totalPostUpVotes = postData[8];
    var totalPostDownVotes = postData[9];
    var totalPostFlags = postData[10];
    var userUpVoted = postData[11];
    var userDownVoted = postData[12];
    var userFavorited = postData[13];
    var userFlagged = postData[14];

    const loggedInScreename = document.querySelector(".screenname").id;
    document.getElementById("insert-messages").classList.add("messages-container");
    var div2 = document.createElement("div");
    div2.classList.add("user-message-container");
    div2.classList.add(postId);
    div2.setAttribute("id", postId);
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
    div12.textContent = `${totalUserContributions}`;
    div12.classList.add('padding-right-5');
    if (screenname == loggedInScreename) {
        div12.classList.add("total-contributions");
        updateTotalCounts(totalUserContributions);
    }
    div15.appendChild(div12);
    var div16 = document.createElement("div");
    div16.classList.add("material-symbols-outlined");
    div16.classList.add("avatar-row-user-badge");
    div16.textContent = "social_leaderboard";
    div15.appendChild(div16);
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
    var div14 = document.createElement("div");
    div14.classList.add('message-colum');
    div2.appendChild(div14);
    var div4 = document.createElement("div");
    div4.classList.add('message-row');
    div14.appendChild(div4);
    var h3 = document.createElement("h3");
    h3.classList.add('message-h3-heading');
    h3.textContent = `Humor:`;
    div4.appendChild(h3);
    var div16 = document.createElement("div");
    div16.classList.add('message-row');
    div2.appendChild(div16);
    var p = document.createElement("p");
    p.classList.add('member-question');
    p.classList.add('message-paragraph');
    p.textContent = `${humor}`;
    div16.appendChild(p);
    var div4 = document.createElement('div');
    div4.classList.add('response-row');
    div2.appendChild(div4);
    var div5 = document.createElement('div');
    div5.classList.add("response-column1");
    if (userUpVoted.localeCompare("1") == 0) { div5.classList.add("user-voted"); }
    div5.setAttribute("id", postId);
    div4.appendChild(div5);
    var div10 = document.createElement('div');
    div10.classList.add("vote-count");
    var span10 = document.createElement('span');
    span10.textContent = `${totalPostUpVotes}`;
    span10.setAttribute("id", postId + "-usr_upVoted");
    div10.appendChild(span10);
    div4.appendChild(div10);
    var div6 = document.createElement('div');
    div6.classList.add("response-column2");
    if (userDownVoted.localeCompare("1") == 0) { div6.classList.add("user-voted"); }
    div6.setAttribute("id", postId);
    div4.appendChild(div6);
    var div11 = document.createElement('div');
    div11.classList.add("vote-count");
    var span11 = document.createElement('span');
    span11.textContent = `${totalPostDownVotes}`;
    span11.setAttribute("id", postId + "-usr_downVoted");
    div11.appendChild(span11);
    div4.appendChild(div11);
    var div7 = document.createElement('div');
    div7.classList.add("response-column5");
    div4.appendChild(div7);
    var div8 = document.createElement('div');
    div8.classList.add("response-column3");
    if (userFavorited.localeCompare("1") == 0) { div8.classList.add("user-voted"); }
    div8.setAttribute("id", postId);
    div4.appendChild(div8);
    var div12 = document.createElement('div');
    div12.classList.add("transparent");
    div12.classList.add("vote-count");
    var span12 = document.createElement('span');
    span12.setAttribute("id", postId + "-usr_stared");
    span12.textContent = "0";
    div12.appendChild(span12);
    div4.appendChild(div12);
    if (userFavorited == "1") {
        span12.textContent = "1";
        div12.classList.remove("transparent");
    }
    var div9 = document.createElement('div');
    div9.classList.add("response-column4");
    if (userFlagged.localeCompare("1") == 0) { div9.classList.add("user-voted"); }
    div9.setAttribute("id", postId);
    div4.appendChild(div9);
    var div13 = document.createElement('div');
    div13.classList.add("vote-count");
    var span13 = document.createElement('span');
    span13.textContent = `${totalPostFlags}`;
    span13.setAttribute("id", postId + "-usr_flagged");
    div13.appendChild(span13);
    div4.appendChild(div13);
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
    img4.classList.add('star-img');
    img4.classList.add('filter-gold');
    img4.src = "/img/star.svg";
    div8.appendChild(img4);
    var img5 = document.createElement('img')
    img5.classList.add('report-img');
    img5.classList.add('filter-red');
    img5.src = "/img/report.svg";
    div9.appendChild(img5);
    document.getElementById("insert-messages").prepend(div2);
    stopSpin();
    initButtonRow();
});
connection.on("ReceiveMessageProblemSolution", function (postDataIn) {
    var postData = JSON.parse(postDataIn);
    var postId = postData[0];
    var truth = postData[1];
    var humor = postData[2];
    var problem = postData[3];
    var solution = postData[4];
    var username = postData[5];
    var screenname = postData[6];
    var totalUserContributions = postData[7];
    var totalPostUpVotes = postData[8];
    var totalPostDownVotes = postData[9];
    var totalPostFlags = postData[10];
    var userUpVoted = postData[11];
    var userDownVoted = postData[12];
    var userFavorited = postData[13];
    var userFlagged = postData[14];

    const loggedInScreename = document.querySelector(".screenname").id;
    document.getElementById("insert-messages").classList.add("messages-container");
    var div2 = document.createElement("div");
    div2.classList.add("user-message-container");
    div2.classList.add(postId);
    div2.setAttribute("id", postId);
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
    div12.textContent = `${totalUserContributions}`;
    div12.classList.add('padding-right-5');
    if (screenname == loggedInScreename) {
        div12.classList.add("total-contributions");
        updateTotalCounts(totalUserContributions);
    }
    div15.appendChild(div12);
    var div16 = document.createElement("div");
    div16.classList.add("material-symbols-outlined");
    div16.classList.add("avatar-row-user-badge");
    div16.textContent = "social_leaderboard";
    div15.appendChild(div16);
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
    var div14 = document.createElement("div");
    div14.classList.add('message-colum');
    div2.appendChild(div14);
    var div4 = document.createElement("div");
    div4.classList.add('message-row');
    div14.appendChild(div4);
    var h3 = document.createElement("h3");
    h3.classList.add('message-h3-heading');
    h3.textContent = `Question:`;
    div4.appendChild(h3);
    var div16 = document.createElement("div");
    div16.classList.add('message-row');
    div2.appendChild(div16);
    var p = document.createElement("p");
    p.classList.add('member-question');
    p.classList.add('message-paragraph');
    p.textContent = `${problem}`;
    div16.appendChild(p);
    var div17 = document.createElement("div");
    div17.classList.add('message-row');
    div2.appendChild(div17);
    var h3b = document.createElement("h3");
    h3b.classList.add('message-h3-heading');
    h3b.textContent = `Answer:`;
    div17.appendChild(h3b);
    var div18 = document.createElement("div");
    div18.classList.add('message-row');
    div2.appendChild(div18);
    var p2 = document.createElement("p");
    p2.classList.add('member-question');
    p2.classList.add('message-paragraph');
    p2.textContent = `${solution}`;
    div18.appendChild(p2);
    var div4 = document.createElement('div');
    div4.classList.add('response-row');
    div2.appendChild(div4);
    var div5 = document.createElement('div');
    div5.classList.add("response-column1");
    if (userUpVoted.localeCompare("1") == 0) { div5.classList.add("user-voted"); }
    div5.setAttribute("id", postId);
    div4.appendChild(div5);
    var div10 = document.createElement('div');
    div10.classList.add("vote-count");
    var span10 = document.createElement('span');
    span10.textContent = `${totalPostUpVotes}`;
    span10.setAttribute("id", postId + "-usr_upVoted");
    div10.appendChild(span10);
    div4.appendChild(div10);
    var div6 = document.createElement('div');
    div6.classList.add("response-column2");
    if (userDownVoted.localeCompare("1") == 0) { div6.classList.add("user-voted"); }
    div6.setAttribute("id", postId);
    div4.appendChild(div6);
    var div11 = document.createElement('div');
    div11.classList.add("vote-count");
    var span11 = document.createElement('span');
    span11.textContent = `${totalPostDownVotes}`;
    span11.setAttribute("id", postId + "-usr_downVoted");
    div11.appendChild(span11);
    div4.appendChild(div11);
    var div7 = document.createElement('div');
    div7.classList.add("response-column5");
    div4.appendChild(div7);
    var div8 = document.createElement('div');
    div8.classList.add("response-column3");
    if (userFavorited.localeCompare("1") == 0) { div8.classList.add("user-voted"); }
    div8.setAttribute("id", postId);
    div4.appendChild(div8);
    var div12 = document.createElement('div');
    div12.classList.add("transparent");
    div12.classList.add("vote-count");
    var span12 = document.createElement('span');
    span12.setAttribute("id", postId + "-usr_stared");
    span12.textContent = "0";
    div12.appendChild(span12);
    div4.appendChild(div12);
    if (userFavorited == "1") {
        span12.textContent = "1";
        div12.classList.remove("transparent");
    }
    var div9 = document.createElement('div');
    div9.classList.add("response-column4");
    if (userFlagged.localeCompare("1") == 0) { div9.classList.add("user-voted"); }
    div9.setAttribute("id", postId);
    div4.appendChild(div9);
    var div13 = document.createElement('div');
    div13.classList.add("vote-count");
    var span13 = document.createElement('span');
    span13.textContent = `${totalPostFlags}`;
    span13.setAttribute("id", postId + "-usr_flagged");
    div13.appendChild(span13);
    div4.appendChild(div13);
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
    img4.classList.add('star-img');
    img4.classList.add('filter-gold');
    img4.src = "/img/star.svg";
    div8.appendChild(img4);
    var img5 = document.createElement('img')
    img5.classList.add('report-img');
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
    div2.classList.add("ai-message-container");
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
    var div14 = document.createElement("div");
    div14.classList.add('message-colum');
    div2.appendChild(div14);
    var div4 = document.createElement("div");
    div4.classList.add('message-row');
    div14.appendChild(div4);
    var h3 = document.createElement("h3");
    h3.classList.add('message-h3-heading');
    h3.textContent = `Question:`;
    div4.appendChild(h3);
    var div16 = document.createElement("div");
    div16.classList.add('message-row');
    div2.appendChild(div16);
    var p = document.createElement("p");
    p.classList.add('member-question');
    p.classList.add('message-paragraph');
    p.textContent = `${query}`;
    div16.appendChild(p);
    var div17 = document.createElement("div");
    div17.classList.add('message-row');
    div2.appendChild(div17);
    var h3b = document.createElement("h3");
    h3b.classList.add('message-h3-heading');
    h3b.textContent = `Response:`;
    div17.appendChild(h3b);
    var div18 = document.createElement("div");
    div18.classList.add('message-row');
    div2.appendChild(div18);
    var p2 = document.createElement("p");
    p2.classList.add('member-question'); 
    p2.classList.add('message-paragraph');
    p2.classList.add('ai-response');
    p2.textContent = `${response}`;
    div18.appendChild(p2);
    var div4 = document.createElement('div');
    div4.classList.add('response-row');
    div2.appendChild(div4);
    var div5 = document.createElement('div');
    div5.classList.add("ai-response-column1");
    div5.setAttribute("id", 'aiThumbsUp');
    div4.appendChild(div5);
    var div6 = document.createElement('div');
    div6.classList.add("ai-response-column2");
    div6.setAttribute("id", id);
    div4.appendChild(div6);
    var div7 = document.createElement('div');
    div7.classList.add("ai-response-column5");
    div4.appendChild(div7);
    var div8 = document.createElement('div');
    div8.classList.add("ai-response-column3");
    div8.setAttribute("id", id);
    div4.appendChild(div8);
    var div9 = document.createElement('div');
    div9.classList.add("ai-response-column4");
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
    var img5 = document.createElement('img')
    img5.classList.add('report-img');
    img5.classList.add('filter-red');
    img5.src = "/img/report.svg";
    div9.appendChild(img5);
    document.getElementById("insert-ai-response").prepend(div2);
    stopSpin();
    initButtonRow();
    //the following must load at the end of this code block after the response columns are in the dom
    var thumbsUp = document.querySelector('.ai-response-column1');
    var thumbsDown = document.querySelector('.ai-response-column2');
    var flag = document.querySelector('.ai-response-column4');
    thumbsUp.addEventListener('click', function () {
        aiFeedbackNotification();
    });
    thumbsDown.addEventListener('click', function () {
        aiFeedbackNotification();
    });
    flag.addEventListener('click', function () {
        aiFeedbackNotification();
    });
});
function aiFeedbackNotification() {
    $.toast({
        text: 'The power behing the collective salutes you!',
        icon: 'info',
        loader: true,
        stack: 4,
        position: 'bottom-center',
        hideAfter: 5000,
    })
}
//update contributions when current user casts a vote
connection.on("UpdateContributions", function (user, updatedContributions) {
    var currentUser = document.querySelector('.jam').id;
    
    if (user == currentUser) {
        updateTotalCounts(updatedContributions);
    }
});

//starts connection and initiates responses.
connection.start().then(function () {

    let currentPage = 1;
    var user = document.querySelector('.jam').id;
    var screenname = document.querySelector('.screenname').id;
    connection.invoke("SendMessages", currentPage, user,);
    onLoadAI();
    var connectionid = connection.connectionId;
    console.log(connectionid);
}).catch(function (err) {
    return console.error(err.toString());
});
const previousPageSelector = document.getElementById('left-nav-next');
const nextPageSelector = document.getElementById('right-nav-next');
const rightNavEnd = document.getElementById('right-nav-end');
const leftNavEnd = document.getElementById('left-nav-end');
function initNav() {    
    previousPageSelector.onclick = function () {
        var connectionId = connection.connectionId;
        var user = document.querySelector('.jam').id;
        var previousText = previousPageSelector.innerText;
        let newPage = parseInt(previousText[0]);
        if (newPage << 0) {
            connection.invoke("AdjustPosts", newPage, user, connectionId).catch(function (err) {
                return console.error(err.toString());
                
            });
        }      
    }
    nextPageSelector.onclick = function () {
        var connectionId = connection.connectionId;
        var user = document.querySelector('.jam').id;
        var nextText = nextPageSelector.innerText;
        let newPage = parseInt(nextText[0]);
        let lastPage = parseInt(nextText[2]);
        if (newPage <= lastPage) {
            connection.invoke("AdjustPosts", newPage, user, connectionId).catch(function (err) {
                return console.error(err.toString());
            });
        } 
    }
    leftNavEnd.onclick = function () {
        var connectionId = connection.connectionId;
        var user = document.querySelector('.jam').id;
        var nextText = previousPageSelector.innerText;
        let nextNewPage = parseInt(nextText[0]);
        let newPage = 1;
        if (nextNewPage == 0) { } else {
            connection.invoke("AdjustPosts", newPage, user, connectionId).catch(function (err) {
                return console.error(err.toString());

            });
        }
    }
    rightNavEnd.onclick = function () {
        var connectionId = connection.connectionId;
        var user = document.querySelector('.jam').id;
        var nextText = nextPageSelector.innerText;
        let newPage = parseInt(nextText[0]);
        let lastPage = parseInt(nextText[2]);
        if (newPage <= lastPage) {
            connection.invoke("AdjustPosts", newPage, user, connectionId).catch(function (err) {
                return console.error(err.toString());
            });
        } 
    }
} 
//prev. - next

//update page numbers durring navigation

connection.on("adjustClientNavPages", function (user, newCurrentPageIn, newTotalPagesIn, postsToKeepList) {
    var newPreviousPage = newCurrentPageIn - 1;
    var newNextPage = newCurrentPageIn + 1;
    console.log("postsToKeep: "+postsToKeepList);
    document.getElementById('previous_page').innerText = newPreviousPage + "|" + newTotalPagesIn;
    document.getElementById('next_page').innerText = newNextPage + "|" + newTotalPagesIn;

    var oldPostsStringList = [];
    const oldPostsClassList = document.querySelectorAll('.user-message-container');
    oldPostsClassList.forEach(elem => { oldPostsStringList.push(elem.id); });
    console.log("Full List: " + oldPostsStringList);

    oldPostsStringList.forEach(elem => {
        if (!postsToKeepList.includes(elem)) {
            console.log(elem);
            document.getElementById(elem).remove();
        }
    });
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
document.getElementById("sendButton2").addEventListener("click", function (event) {    
    var user = document.querySelector('.jam').id;
    var screenname = document.querySelector('.screenname').id;
    const tabs = document.querySelectorAll('.tab');
    var connectionId = connection.connectionId;
    console.log(connectionId);
    startSpin();    
    if (tabs[3].classList.contains('active')) {
        var queryAI = document.getElementById("messageInputAI").value;
        let postId = "0";
        connection.invoke("ProccessAI", user, queryAI, postId, screenname, connectionId).catch(function (err) {
            return console.error(err.toString());
        });
    }
    event.preventDefault();
    eraseText();
});
const select_io = document.querySelector('.angled-left');
const select_ai = document.querySelector('.angled-right');
select_io.addEventListener('click', function () {    

    hideBanner();
    if (document.getElementById('bopis').checked == false) {
        toggle_button.click();
    }
});
select_ai.addEventListener('click', function () {
    hideBanner();
    if (document.getElementById('bopis').checked == true) {
        toggle_button.click();
    }
});
//method only controls imediate vote results in the dom.  Server fed value are pushed on refresh.
function updateCurrentPostVote(postId, qtyAdj, voteType) {

    if (voteType == "UpVoted") {
        var upVotedElementId = postId + "-usr_upVoted";
        var upVoteElement = document.getElementById(upVotedElementId);
        var currentCount = parseInt(upVoteElement.textContent);
        var newCount = currentCount + qtyAdj;
        upVoteElement.textContent = newCount;
        //adjusting user-voted tags must be done in the calling class
    } else if (voteType == "DownVoted") {
        var downVotedElementId = postId + "-usr_downVoted";
        var downVoteElement = document.getElementById(downVotedElementId);
        var currentCount = parseInt(downVoteElement.textContent);
        var newCount = currentCount + qtyAdj;
        downVoteElement.textContent = newCount;
        //adjusting user-voted tags must be done in the calling class
    } else if (voteType == "StarVoted") {
        var staredElementId = postId + "-usr_stared";
        var staredElement = document.getElementById(staredElementId);
        var currentValue = staredElement.textContent;
        console.log(staredElementId)
        console.log(postId)
        console.log(staredElement.textContent)
        if (currentValue == "0") {
            staredElement.textContent = "1";
            document.querySelector('.response-column3').classList.add("user-voted"); 6
            document.getElementById(staredElementId).parentNode.classList.remove('transparent');
        } else if (currentValue == "1") {
            staredElement.textContent = "0";
            document.querySelector('.response-column3').classList.remove("user-voted");
            document.getElementById(staredElementId).parentNode.classList.add('transparent');
        }
    } else if (voteType == "Flagged") {
        var flaggedElementId = postId + "-usr_flagged";
        var flaggedElement = document.getElementById(flaggedElementId);
        var currentCount = parseInt(flaggedElement.textContent);
        var newCount = currentCount + qtyAdj;
        flaggedElement.textContent = newCount;
        //adjusting user-voted tags must be done in the calling class
    }
}

//post response buttons
function initButtonRow() {
    var screenname = document.querySelector('.screenname').id;
    var pepper = document.querySelector('.pepper').id;
    var jam = document.querySelector('.jam').id;
    var col1 = document.querySelector('.response-column1');
    var col2 = document.querySelector('.response-column2');
    var col3 = document.querySelector('.response-column3');
    var col4 = document.querySelector('.response-column4');
    var usr_upVoted = false;
    var usr_downVoted = false;
    var usr_stared = false;
    var usr_flagged = false;
    if (col1.classList.contains("user-voted")) { usr_upVoted = true; }
    if (col2.classList.contains("user-voted")) { usr_downVoted = true; }
    if (col3.classList.contains("user-voted")) { usr_stared = true; }
    if (col4.classList.contains("user-voted")) { usr_flagged = true; }
    //updates the logic tree for dom user-voted element id tags to comprehend various vote states 
    function varRefresh() {
        if (col1.classList.contains("user-voted")) { usr_upVoted = true; }
        if (col2.classList.contains("user-voted")) { usr_downVoted = true; }
        if (col3.classList.contains("user-voted")) { usr_stared = true; }
        if (col4.classList.contains("user-voted")) { usr_flagged = true; }
    }
    //up vote
    col1.onclick = function () {
        loginNotification();
        var screenName = document.querySelector(".screenname").id;
        if (screenName == 'Anonymous') { }
        else {
            console.log("Col1 UpVoted Clicked!")
            var pepperOut = pepper.toString();
            var userNameOut = jam.toString();

            const messageIdOut = col1.id.toString();
            var screenNameOut = screenname;

            if (usr_upVoted || usr_downVoted || usr_flagged) {
                if (usr_upVoted) {
                    var voteTypeOut = "UpVoted";
                    let qtyAdjOut = -1;
                    updateCurrentPostVote(messageIdOut, qtyAdjOut, voteTypeOut);
                    col1.classList.remove('user-voted');
                    connection.invoke("RemoveVote", voteTypeOut, pepperOut, userNameOut, messageIdOut, screenNameOut).catch(function (err) {
                        return console.error(err.toString());
                    });
                } else if (usr_downVoted) {
                    var voteTypeOut = "DownVoted";
                    let qtyAdjOut = -1;
                    updateCurrentPostVote(messageIdOut, qtyAdjOut, voteTypeOut);
                    col2.classList.remove('user-voted');
                    connection.invoke("RemoveVote", voteTypeOut, pepperOut, userNameOut, messageIdOut, screenNameOut).catch(function (err) {
                        return console.error(err.toString());
                    });
                } else if (usr_flagged) {
                    var voteTypeOut = "Flagged";
                    let qtyAdjOut = -1;
                    updateCurrentPostVote(messageIdOut, qtyAdjOut, voteTypeOut);
                    col4.classList.remove('user-voted');
                    connection.invoke("RemoveVote", voteTypeOut, pepperOut, userNameOut, messageIdOut, screenNameOut).catch(function (err) {
                        return console.error(err.toString());
                    });
                }
            }
            //the usr_upVoted state is past tense here. It was set prior to the user current vote. This refrains from casting a vote is  it's no necessary due to an un-vote occurance. It also allows a new vote to be cast if the post was not already voted for by the user.
            if (usr_upVoted) {
                usr_upVoted = false;
                col1.classList.remove('.user-voted');
            } else {
                var voteTypeOut = "UpVoted";
                let qtyAdjOut = 1;
                updateCurrentPostVote(messageIdOut, qtyAdjOut, voteTypeOut);
                col1.classList.add('user-voted');
                col2.classList.remove('user-voted');
                col4.classList.remove('user-voted');
                usr_upVoted = true;
                usr_downVoted = false;
                usr_flagged = false;
                connection.invoke("CastVote", voteTypeOut, pepperOut, userNameOut, messageIdOut, screenNameOut).catch(function (err) {
                    return console.error(err.toString());
                });
            }
        }//end soft login check
    }
    //down vote
    col2.onclick = function () {
        loginNotification();
        var screenName = document.querySelector(".screenname").id;
        if (screenName == 'Anonymous') { }
        else {
            console.log("Col2 DownVoted Clicked!")
            varRefresh();
            var pepperOut = pepper.toString();
            var userNameOut = jam.toString();
            const messageIdOut = col2.id.toString();
            var screenNameOut = screenname;

            if (usr_upVoted || usr_downVoted || usr_flagged) {
                if (usr_upVoted) {
                    var voteTypeOut = "UpVoted";
                    let qtyAdjOut = -1;
                    updateCurrentPostVote(messageIdOut, qtyAdjOut, voteTypeOut);
                    col1.classList.remove('user-voted');
                    connection.invoke("RemoveVote", voteTypeOut, pepperOut, userNameOut, messageIdOut, screenNameOut).catch(function (err) {
                        return console.error(err.toString());
                    });
                } else if (usr_downVoted) {
                    var voteTypeOut = "DownVoted";
                    let qtyAdjOut = -1;
                    updateCurrentPostVote(messageIdOut, qtyAdjOut, voteTypeOut);
                    col2.classList.remove('user-voted');
                    connection.invoke("RemoveVote", voteTypeOut, pepperOut, userNameOut, messageIdOut, screenNameOut).catch(function (err) {
                        return console.error(err.toString());
                    });
                } else if (usr_flagged) {
                    var voteTypeOut = "Flagged";
                    let qtyAdjOut = -1;
                    updateCurrentPostVote(messageIdOut, qtyAdjOut, voteTypeOut);
                    col4.classList.remove('user-voted');
                    connection.invoke("RemoveVote", voteTypeOut, pepperOut, userNameOut, messageIdOut, screenNameOut).catch(function (err) {
                        return console.error(err.toString());
                    });
                }
            }
            //the usr_downVoted state is past tense here. It was set prior to the users current vote. This refrains from casting a vote is it's no necessary due to an un-vote occurance. It also allows a new vote to be cast if the post was not already voted for by the user.
            if (usr_downVoted) {
                usr_downVoted = false;
                col2.classList.remove('.user-voted');
            } else {
                var voteTypeOut = "DownVoted";
                let qtyAdjOut = 1;
                updateCurrentPostVote(messageIdOut, qtyAdjOut, voteTypeOut);
                col2.classList.add('user-voted');
                col1.classList.remove('user-voted');
                col3.classList.remove('user-voted');

                usr_upVoted = false;
                usr_downVoted = true;
                usr_flagged = false;
                connection.invoke("CastVote", voteTypeOut, pepperOut, userNameOut, messageIdOut, screenNameOut).catch(function (err) {
                    return console.error(err.toString());
                });
            }
        }//end soft login check
    }
    //stared
    col3.onclick = function () {
        loginNotification();

        if (screenName == 'Anonymous') { }
        else {
            console.log("Col3 Stared Clicked!")
            varRefresh();
            var voteTypeOut = "StarVoted";
            var pepperOut = pepper.toString();
            var userNameOut = jam.toString();
            var screenName = document.querySelector(".screenname").id;
            const messageIdOut = col3.id.toString();
            var screenNameOut = screenname;

            if (usr_stared) {
                var qtyAdjOut = -1;
                updateCurrentPostVote(messageIdOut, qtyAdjOut, voteTypeOut);
                connection.invoke("RemoveVote", voteTypeOut, pepperOut, userNameOut, messageIdOut, screenNameOut).catch(function (err) {
                    return console.error(err.toString());
                });
            } else if (!usr_stared) {
                var qtyAdjOut = 1;
                updateCurrentPostVote(messageIdOut, qtyAdjOut, voteTypeOut);
                connection.invoke("CastVote", voteTypeOut, pepperOut, userNameOut, messageIdOut, screenNameOut).catch(function (err) {
                    return console.error(err.toString());
                });
            }
        }//end soft login check
    }
    //flagged vote
    col4.onclick = function () {
        loginNotification();
        var screenName = document.querySelector(".screenname").id;
        if (screenName == 'Anonymous') { }
        else {
            console.log("Col4 Flagged Clicked!")
            varRefresh();
            var pepperOut = pepper.toString();
            var userNameOut = jam.toString();
            const messageIdOut = col4.id.toString();
            var screenNameOut = screenname;

            if (usr_upVoted || usr_downVoted || usr_flagged) {
                if (usr_upVoted) {
                    var voteTypeOut = "UpVoted";
                    let qtyAdjOut = -1;
                    updateCurrentPostVote(messageIdOut, qtyAdjOut, voteTypeOut);
                    col1.classList.remove('user-voted');
                    connection.invoke("RemoveVote", voteTypeOut, pepperOut, userNameOut, messageIdOut, screenNameOut).catch(function (err) {
                        return console.error(err.toString());
                    });
                } else if (usr_downVoted) {
                    var voteTypeOut = "DownVoted";
                    let qtyAdjOut = -1;
                    updateCurrentPostVote(messageIdOut, qtyAdjOut, voteTypeOut);
                    col2.classList.remove('user-voted');
                    connection.invoke("RemoveVote", voteTypeOut, pepperOut, userNameOut, messageIdOut, screenNameOut).catch(function (err) {
                        return console.error(err.toString());
                    });
                } else if (usr_flagged) {
                    var voteTypeOut = "Flagged";
                    let qtyAdjOut = -1;
                    updateCurrentPostVote(messageIdOut, qtyAdjOut, voteTypeOut);
                    col4.classList.remove('user-voted');
                    connection.invoke("RemoveVote", voteTypeOut, pepperOut, userNameOut, messageIdOut, screenNameOut).catch(function (err) {
                        return console.error(err.toString());
                    });
                }
            }
            //the usr_flagged state is past tense here. It was set prior to the users current vote. This refrains from casting a vote is  it's no necessary due to an un-vote occurance. It also allows a new vote to be cast if the post was not already voted for by the user.
            if (usr_flagged) {
                usr_flagged = false;
                col4.classList.remove('.user-voted');
            } else {
                var voteTypeOut = "Flagged";
                let qtyAdjOut = 1;
                updateCurrentPostVote(messageIdOut, qtyAdjOut, voteTypeOut);
                col4.classList.add('user-voted');
                col1.classList.remove('user-voted');
                col2.classList.remove('user-voted');
                usr_upVoted = false;
                usr_downVoted = false;
                usr_flagged = true;
                connection.invoke("CastVote", voteTypeOut, pepperOut, userNameOut, messageIdOut, screenNameOut).catch(function (err) {
                    return console.error(err.toString());
                });
            }
        }//end soft login check        

    }
    // Toast functions
    function loginNotification() {
        var element = document.querySelector(".screenname").id;

        if (element == 'Anonymous') {
            $.toast({
                heading: 'Login Required',
                text: 'Login required to vote',
                icon: 'info',
                loader: true,
                stack: 4,
                position: 'bottom-center',
                hideAfter: 5000,
            })
        } else {
            $.toast({
                heading: 'Submitted',
                text: 'The power behing the collective salutes you! *Note: Only one vote per user will be retained.',
                icon: 'success',
                loader: true,
                stack: 4,
                position: 'bottom-center',
                hideAfter: 5000,
            })
        }
    }

    let user = document.querySelector('.username').id;
    var aiResponse = document.querySelector(".ai-response");
    $(aiResponse).typewrite({
        'delay': 1, //time in ms between each letter
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
//Invokes initial AI message from the server.
function invokeIAHello() {
    
    var user = document.querySelector('.jam').id;
    var screenname = document.querySelector('.screenname').id;
    var connectionId = connection.connectionId;
    connection.invoke("helloAI", user, screenname, connectionId).catch(function (err) {
        return console.error(err.toString());
    });
}
//make sure AI messages are hidden on page reload
function hideAIResponsContainer() {
    const aiMessaagesClass = document.querySelectorAll('.ai-container');
    aiMessaagesClass.forEach(elem => { elem.classList.add('hidden') });
}
hideAIResponsContainer();
//hide old ai messages after toggle
function hideAIResponses() {
    const aiMessaagesClass = document.querySelectorAll('.ai-message-container');
    aiMessaagesClass.forEach(elem => { elem.classList.add('hidden') });
    const postPageNave = document.querySelectorAll('.nav-post-range-col');
    postPageNave.forEach(elem => { elem.classList.add('hidden') });

}
//hide or reveal messages
function hidePostMessages() {
    const postMessaagesClass = document.querySelectorAll('.messages-container');
    postMessaagesClass.forEach(elem => { elem.classList.add('hidden') });
}
function revealPostMessages() {
    const postMessaagesClass = document.querySelectorAll('.messages-container');
    postMessaagesClass.forEach(elem => { elem.classList.remove('hidden') });
    hideAIMessages();
    const postPageNave = document.querySelectorAll('.nav-post-range-col');
    postPageNave.forEach(elem => { elem.classList.remove('hidden') });
    initNav();

    pageNumUpdate();
}
function pageNumUpdate() {
    connection.invoke("GetPages").catch(function (err) {
        return console.error(err.toString());
    });
}
connection.on("ReceivTotalAvailablePageCount", function (newTotalPagesIn) {
    document.getElementById('previous_page').innerText = "0|" + newTotalPagesIn;
    document.getElementById('next_page').innerText = "2|" + newTotalPagesIn;
});

function hideAIMessages() {
    const aiMessaagesClass = document.querySelectorAll('.ai-container');
    aiMessaagesClass.forEach(elem => { elem.classList.add('hidden') });
}
function revealAIMessages() {
    const aiMessaagesClass = document.querySelectorAll('.ai-container');
    aiMessaagesClass.forEach(elem => { elem.classList.remove('hidden') });
    hideAIResponses(); //creates a fresh start
    invokeIAHello();
}
const messageInputAI = document.querySelectorAll('.messageInputAI');
function showIO() {
    hideAIMessages();
    revealPostMessages();
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
document.getElementById("bopis").checked = true;
const toggle_button = document.querySelector('.on-off-toggle__input');
const card = document.getElementById('card__inner');
var intro_text = document.getElementById('intro-text');
var intro_ai = document.getElementById('intro_ai');
var intro_io = document.getElementById('intro_io');
intro_text.classList.add('intro-hidden');
intro_io.classList.add('intro-hidden');
intro_ai.classList.add('intro-un-hidden');
toggle_button.addEventListener('click', function () {
    eraseText();
    hideBanner();

    card.classList.toggle('is--flipped');


    //this is the ai reveal
    if (card.classList.contains('is--flipped')) {
        
        hidePostMessages();
        revealAIMessages();
        intro_text.classList.remove('intro-un-hidden');
        intro_text.classList.add('intro-hidden');
        
        intro_ai.classList.add('intro-un-hidden');
        intro_ai.classList.remove('intro-hidden');
        intro_io.classList.remove('intro-un-hidden');
        intro_io.classList.add('intro-hidden');
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
    //this is the io reveal
    if (!card.classList.contains('is--flipped')) {
        
        hideAIMessages();
        revealPostMessages();
        intro_text.classList.remove('intro-hidden');
        intro_text.classList.add('intro-un-hidden');
        intro_io.classList.add('intro-un-hidden');
        intro_io.classList.remove('intro-hidden');
        intro_ai.classList.remove('intro-un-hidden');
        intro_ai.classList.add('intro-hidden');
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
function showAI() {
    hideAIResponses();
    hidePostMessages();
    revealAIMessages();
    indicators.forEach(indicator => { indicator.classList.remove('active') });
    indicators[3].classList.add('active');
    messageInputAI.forEach(elem => { elem.classList.remove('hide_textarea') });
    contents.forEach(content => { content.classList.remove('active') });
    contents.forEach(content => { content.classList.remove('hide_textarea') });
    contents[0].classList.add('hide_textarea');
    contents[1].classList.add('hide_textarea');
    contents[2].classList.add('hide_textarea');
    contents[3].classList.add('hide_textarea');
    contents[4].classList.add('active');
    tabs[3].classList.add('active');
}
function onLoadAI() {
    eraseText();
    if (card.classList.contains('is--flipped')) {
        showAI();
    }
    if (!card.classList.contains('is--flipped')) {
        card.classList.toggle('is--flipped');
        document.getElementById("bopis").checked = false;
        showAI();
    }
}
