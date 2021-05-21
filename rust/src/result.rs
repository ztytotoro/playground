#[derive(Debug)]
struct Error;

pub fn test() {
    println!("{:?}", a(false));
}

fn a(val: bool) -> Result<bool, Error> {
    if val {
        Ok(val)
    } else {
        Err(Error)
    }
}