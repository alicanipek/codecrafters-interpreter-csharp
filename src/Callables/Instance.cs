public class Instance {
    private readonly Cls _cls;
    public Instance(Cls cls) {
        _cls = cls;
    }

    public override string ToString() {
        return _cls.Name + " instance";
    }

}
