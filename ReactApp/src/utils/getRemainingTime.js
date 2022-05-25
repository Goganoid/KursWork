function getRemainingTime(endDate, startDate = null) {

    if (startDate == null) startDate = new Date().getTime();

    // Find the distance between now and the count down date
    let distance = endDate - startDate;
    if (distance < 0) distance = 0;
    // Time calculations for days, hours, minutes and seconds
    return {
        days: Math.floor(distance / (1000 * 60 * 60 * 24)),
        hours: Math.floor((distance % (1000 * 60 * 60 * 24)) / (1000 * 60 * 60)),
        minutes: Math.floor((distance % (1000 * 60 * 60)) / (1000 * 60)),
        seconds: Math.floor((distance % (1000 * 60)) / 1000),

    }
}

export default getRemainingTime