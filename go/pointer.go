package main

type Pointer struct {
	Index int
	List  []interface{}
}

func NewPointer(list []interface{}) *Pointer {
	return &Pointer{
		Index: 0,
		List:  list,
	}
}

func (p *Pointer) Current() interface{} {
	return p.List[p.Index]
}

func (p *Pointer) Move(step int) bool {
	newIndex := p.Index + step
	if newIndex < 0 || newIndex >= len(p.List) {
		return false
	}
	p.Index = newIndex
	return true
}
