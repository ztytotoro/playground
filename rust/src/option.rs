pub fn test() {
    let v = Some(100);
    println!("{:?}", v);

    if let Some(100) = v {
        println!("v is 100");
    }
}